---
name: talk-abstracts
description: Write conference talk proposals and session descriptions — titles, abstracts, key takeaways, audience level, speaker bios, and private notes for reviewers. Use when responding to a call for papers (CFP), writing a meetup, webinar, or workshop description, or turning an existing talk into a proposal.
user-invocable: false
---

# Talk Abstracts and CFP Submissions

## When to Use This Skill

- Submitting a talk or workshop to a conference call for papers (CFP)
- Writing a session description for a meetup, webinar, or internal tech talk
- Reworking a rejected proposal, or adapting an existing talk for a new event
- Writing or refreshing a speaker bio

---

## What Reviewers Look For

Program committees read hundreds of proposals, often skimming. A proposal wins when it answers, fast:

1. **Who is this for?** A named audience and an honest level.
2. **What will they leave with?** Specific, usable takeaways — not "an overview of X".
3. **Why this speaker?** Evidence you have done the thing, not just read about it.
4. **Why this, here, now?** Fit with the event's theme and audience; a fresh angle on a popular topic.
5. **Is it a sales pitch?** Vendor or product pitches are rejected unless the event asked for them.

---

## The Parts of a Proposal

### Title

- Promise an outcome or provoke a question in about 8 words or fewer.
- Include the searchable keyword attendees will scan for (e.g. "Modular Monolith", "OpenTelemetry").
- Clever is fine; obscure is not. A pun that hides the topic loses to a plain title.
- Patterns that work: "*X* Without *Y*", "What *N* Years of *X* Taught Us About *Y*", "The Hidden Cost of *X*", "*X* Is Not What You Think".
- **Test:** would the target attendee know from the title alone whether the talk is for them?

### Abstract (public)

Three short paragraphs, within the CFP's word limit (usually 100–250 words). Write to the attendee as "you":

1. **Hook** — a problem the audience recognizes from their own work (the `storytelling` skill's hook patterns apply).
2. **Promise** — what the talk covers and the approach, concretely.
3. **Payoff** — what they will be able to do afterward.

```text
[Hook: the pain, in the attendee's words.] [Why the obvious fix doesn't work.]

In this session, you'll [concrete approach], using [real example / case study / live demo]. We'll
cover [2–3 specific topics].

You'll leave knowing how to [takeaway 1], [takeaway 2], and [takeaway 3].
```

Avoid opening with "In this talk I will…", stacking buzzwords, or promising more than fits in the slot.

### Key Takeaways

Three bullets, each starting with a verb and specific enough to check:

| Weak | Strong |
|------|--------|
| Understand microservices | Identify the three signals that a service boundary is in the wrong place |
| Learn about testing | Replace brittle UI tests with a handful of contract tests |
| See best practices for logging | Add structured, correlated logs to an ASP.NET Core API in under an hour |

### Audience and Level

Name the role and the prerequisite knowledge ("developers who have shipped at least one service to production"). Label the level honestly — an "intermediate" talk that spends ten minutes on basics disappoints both groups.

### Speaker Bio

- Third person, 50–100 words. Keep a one-sentence version too; many events ask for both.
- Lead with the credibility relevant to *this* talk, not your full résumé.
- One human detail makes you memorable; one is enough.
- End with where to find you (site or social handle).

### Notes for Reviewers (private)

The field most submitters leave empty — and the one that often decides close calls:

- A timed outline (e.g. "0–5 hook, 5–20 approach, 20–35 demo, 35–45 lessons")
- Where you have given the talk before, with recording links, or why it's new
- For demos: what is live, what is recorded, and the fallback
- Why this event's audience specifically
- Anything that changed since the talk was last given

---

## Worked Example

```text
Title:     Modular Monoliths: The Architecture You Should Have Started With

Abstract:  Your team split the monolith into microservices to move faster. Now a single feature
           touches six repositories, three pipelines, and a distributed transaction nobody
           trusts. The problem wasn't the monolith — it was the missing module boundaries.

           In this session, you'll see how to carve an existing .NET application into modules
           with enforced boundaries, using a real codebase refactored live. We'll cover finding
           the seams, keeping modules from reaching into each other's data, and testing the
           boundaries so they stay put.

           You'll leave knowing how to spot the modules hiding in your code, enforce their
           boundaries with tests, and decide — with evidence — when a module has earned its
           own service.

Takeaways: - Identify candidate module boundaries from change history and data ownership
           - Enforce boundaries with architecture tests that fail the build
           - Apply a checklist for when extracting a service is worth the cost

Level:     Intermediate — developers comfortable with ASP.NET Core and dependency injection
```

---

## Common Mistakes

| Mistake | Fix |
|---------|-----|
| Abstract describes the speaker's journey, not the attendee's benefit | Rewrite the payoff paragraph in the second person |
| Takeaways are topics ("CQRS") instead of abilities | Start each with a verb the attendee will be able to do |
| One proposal sent unchanged to every event | Adjust the hook and the reviewer notes to each event's audience |
| Title needs the abstract to make sense | Put the keyword and the promise in the title itself |
| Scope too big for the slot | Cut to the one idea you can land fully; mention the rest as follow-up |
| Empty reviewer notes | Add the timed outline and prior deliveries |

## Checklist

- [ ] Title passes the "is this for me?" test and contains the searchable keyword
- [ ] Abstract follows hook → promise → payoff and fits the word limit
- [ ] Three verb-first, checkable takeaways
- [ ] Audience and level stated honestly
- [ ] Bio leads with relevant credibility; short and long versions ready
- [ ] Reviewer notes include a timed outline and prior deliveries or recordings
