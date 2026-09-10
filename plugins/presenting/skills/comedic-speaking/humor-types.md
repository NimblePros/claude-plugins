# Types of Humor That Work in Tech Talks

Seven styles, each with an example and the situation it suits. For fill-in templates that build
jokes in these styles, see [joke-formats.md](joke-formats.md).

---

## Self-Deprecating

Point the punchline at yourself, your past mistakes, or your naivety. Never at the audience.

> "I confidently told my team that this new architecture would cut latency in half. It did. Unfortunately, it also quadrupled the error rate, so — great trade."

**Use when:** You have credibility and want to show intellectual humility. Disarms skeptics.

---

## Observational

Name something the audience universally experiences but rarely says out loud.

> "Every microservices diagram in every conference talk looks elegant and clean. Every actual production system looks like someone sneezed on a spider web."

**Use when:** You want to build immediate audience rapport. Works best when the observation is specific and shared.

---

## Callback

Reference something from earlier in the talk — an image, a punchline, a phrase — and bring it back at an unexpected moment.

> *(Earlier: slide shows a dumpster fire icon for legacy code)*
> *(Later: new microservices diagram slowly fades into the same dumpster fire icon)*

**Use when:** You've been talking for 20+ minutes and want to reward attentive audience members. Callbacks signal confidence and craft.

---

## Rule of Three with a Twist

List two expected items, then a third that subverts the pattern.

> "There are three things that will kill your microservices migration: network latency, team alignment, and someone pushing to production on a Friday afternoon."

**Use when:** Making a list. Applies almost anywhere. High reliability, low setup cost.

---

## Misdirection / Subverted Expectation

Set up an expectation, then deliver something unexpected.

> "After six months of work, our team finally achieved our goal. We proved conclusively that it was impossible."

**Use when:** You want a sharp, memorable punchline. Requires careful setup — the expectation must be clearly established.

---

## Absurdist / Deadpan

Treat something ridiculous with complete seriousness, or present something accurate as if it were absurd.

> "Our deployment process has 47 steps. Steps 12 through 18 are optional, but skipping them causes step 34 to fail. We haven't documented which steps those are."

**Use when:** The technical reality is already absurd. Deadpan delivery amplifies the comedy — let the words do the work.

---

## Visual Gag

A slide, image, diagram, or graph that is itself the punchline.

- A perfectly drawn architecture diagram that slowly adds more boxes until illegible
- A "Before" and "After" where After looks worse
- A confidence graph that peaks before the project starts and drops off a cliff after

**Use when:** Your audience responds to visuals, or in virtual and recorded talks where you can't hear the room. Often lands even with international audiences where wordplay doesn't translate.

In reveal.js, fragments and auto-animate are natural tools for visual gags: reveal the punchline element as the last fragment, or morph the "clean" diagram into the messy one with `data-auto-animate` (see the `revealjs` skill).
