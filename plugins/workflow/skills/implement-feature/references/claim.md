# Step 3 — Claim the issue

Claim only the issue that triage settled on. After a pivot, that is the sub-issue, not the
parent.

1. Self-assign it, or confirm you are already assigned:

   ```bash
   gh issue edit <number> --add-assignee @me
   ```

2. Comment that you are taking ownership, and link the plan file you are about to write.
   Name it using the convention in [plan.md](plan.md):

   > I (this user's agent) am taking ownership of this issue and will be implementing it.
   > The implementation plan is documented in
   > `<plan directory>/YYYY-MM-DD-issue-NNN-short-slug.md`.

3. Add the `agent-assigned` label. If the label does not exist, create it first:

   ```bash
   gh label create agent-assigned --description "Claimed by a coding agent" 2>/dev/null
   gh issue edit <number> --add-label agent-assigned
   ```

Next: [plan.md](plan.md).
