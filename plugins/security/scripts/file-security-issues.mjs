#!/usr/bin/env node
// Files the results of a security review (/security:audit) as GitHub issues: one epic plus one sub-issue per finding.
//
//   node "${CLAUDE_PLUGIN_ROOT}/scripts/file-security-issues.mjs" <findings.json> [--dry-run] [--repo owner/name]
//
// findings.json:
// {
//   "title": "Epic: security review 2026-11-19",          // optional; defaults to today's date
//   "commit": "73db02a",                                  // the commit that was reviewed
//   "branch": "main",                                     // optional; the branch that was reviewed (default main)
//   "scope": "authn/authz, injection, web-facing ...",    // one line: what was covered
//   "sound": ["PATs hashed and compared in constant time", ...],   // checked and found sound
//   "notReviewed": ["staff impersonation sessions", ...],           // what the time box left out
//   "findings": [
//     { "title": "Pin azure/login to a commit SHA", "severity": "medium",
//       "area": "CI/CD supply chain", "where": "`.github/workflows/deploy-dev.yml:119` ...",
//       "problem": "...", "exploit": "...", "fix": "..." }
//   ]
// }
//
// Every issue gets the `security` label; sub-issues also get `severity:<critical|high|medium|low>`. Labels are created
// if missing. Findings are filed most severe first. Requires Node.js 18+ and the GitHub CLI (`gh`), signed in.

import { execFileSync } from "node:child_process";
import { readFileSync, writeFileSync, mkdtempSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";

const SEVERITIES = ["critical", "high", "medium", "low"];
const COLORS = { critical: "b60205", high: "d93f0b", medium: "fbca04", low: "0e8a16" };

const args = process.argv.slice(2);
const dryRun = args.includes("--dry-run");
const repoIndex = args.indexOf("--repo");
const file = args.find((a, i) => !a.startsWith("--") && args[i - 1] !== "--repo");
if (!file) {
  console.error("usage: file-security-issues.mjs <findings.json> [--dry-run] [--repo owner/name]");
  process.exit(2);
}

const gh = (...a) => execFileSync("gh", a, { encoding: "utf8" }).trim();
const report = JSON.parse(readFileSync(file, "utf8"));
const findings = [...(report.findings ?? [])];

for (const f of findings) {
  if (!SEVERITIES.includes(f.severity)) throw new Error(`"${f.title}": severity must be one of ${SEVERITIES.join(", ")}`);
  for (const key of ["title", "where", "problem", "exploit", "fix"]) {
    if (!f[key]) throw new Error(`"${f.title ?? "(untitled)"}": missing "${key}"`);
  }
}
findings.sort((a, b) => SEVERITIES.indexOf(a.severity) - SEVERITIES.indexOf(b.severity));

const scratch = mkdtempSync(join(tmpdir(), "security-issues-"));
const bodyFile = (name, text) => {
  const path = join(scratch, name);
  writeFileSync(path, text);
  return path;
};

const findingBody = (f) =>
  [
    `**Severity:** ${f.severity}${f.area ? ` · **Area:** ${f.area}` : ""}`,
    `**Where:** ${f.where}`,
    `**Problem:** ${f.problem}`,
    `**Exploit:** ${f.exploit}`,
    `**Fix:** ${f.fix}`,
  ].join("\n\n");

const counts = SEVERITIES.map((s) => `${findings.filter((f) => f.severity === s).length} ${s}`).join(", ");
const epicBody = (numbers) =>
  [
    `Time-boxed security review of \`${report.branch ?? "main"}\`${report.commit ? ` @ ${report.commit}` : ""}${report.scope ? `, covering ${report.scope}` : ""}.`,
    `**Result:** ${findings.length} confirmed findings — ${counts}.`,
    "## Findings (by severity)",
    findings.map((f, i) => `- [ ] ${numbers[i] ? `#${numbers[i]}` : "(new)"} — **${f.severity}** — ${f.title}`).join("\n") || "None.",
    report.sound?.length ? `## Checked and found sound\n${report.sound.map((s) => `- ${s}`).join("\n")}` : "",
    report.notReviewed?.length ? `## Not reviewed in the time box\n${report.notReviewed.map((s) => `- ${s}`).join("\n")}` : "",
  ]
    .filter(Boolean)
    .join("\n\n");

const title = report.title ?? `Epic: security review ${new Date().toISOString().slice(0, 10)}`;

if (dryRun) {
  console.log(`# ${title}  [security]\n\n${epicBody([])}\n`);
  for (const f of findings) console.log(`---\n# Security: ${f.title}  [security, severity:${f.severity}]\n\n${findingBody(f)}\n`);
  process.exit(0);
}

const repo = repoIndex >= 0 ? args[repoIndex + 1] : gh("repo", "view", "--json", "nameWithOwner", "-q", ".nameWithOwner");

gh("label", "create", "security", "--color", "b60205", "--description", "Security finding", "--force", "-R", repo);
for (const s of SEVERITIES) {
  gh("label", "create", `severity:${s}`, "--color", COLORS[s], "--description", `Security finding severity: ${s}`, "--force", "-R", repo);
}

const numbers = [];
for (const [i, f] of findings.entries()) {
  const url = gh("issue", "create", "-R", repo, "--title", `Security: ${f.title}`,
    "--body-file", bodyFile(`finding-${i}.md`, findingBody(f)), "--label", "security", "--label", `severity:${f.severity}`);
  numbers.push(url.split("/").pop());
  console.log(`${f.severity.padEnd(8)} ${url}`);
}

const epicUrl = gh("issue", "create", "-R", repo, "--title", title, "--body-file", bodyFile("epic.md", epicBody(numbers)), "--label", "security");
const epic = epicUrl.split("/").pop();
console.log(`epic     ${epicUrl}`);

for (const n of numbers) {
  const id = gh("api", `repos/${repo}/issues/${n}`, "-q", ".id");
  gh("api", "-X", "POST", `repos/${repo}/issues/${epic}/sub_issues`, "-F", `sub_issue_id=${id}`, "--silent");
}
console.log(`linked ${numbers.length} sub-issues to #${epic}`);
