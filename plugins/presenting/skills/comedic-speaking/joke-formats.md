# Joke Formats — Reusable Templates for Technical Presentations

A reference for building jokes in common formats. Each entry includes the structure, a fill-in template, and a worked example.

---

## 1. Rule of Three with Twist

**Structure:** Two expected items → one subversive third.

**Template:**
> "There are three things that will [outcome]: [expected A], [expected B], and [unexpected/honest C]."

**Worked examples:**

> "There are three lies every developer tells: 'It works on my machine,' 'I'll add the tests later,' and 'This will only take five minutes.'"

> "We had three goals for the migration: reduce costs, improve reliability, and finish before the holidays. We achieved one of them. It was not the holiday one."

> "Every architecture review ends the same three ways: approval, rejection, or the decision to form a committee."

---

## 2. The False Success

**Structure:** Build toward a success. Reveal the success is hollow or beside the point.

**Template:**
> "After [duration/effort], we finally achieved [goal]. Unfortunately, [ironic consequence]."

**Worked examples:**

> "After six months of work, we finally had zero critical bugs in our backlog. We achieved this by reclassifying them all as 'enhancements.'"

> "Our test coverage hit 95%. We celebrated. Then we found the 5% was the payment processor."

> "The migration completed on schedule. The schedule had been extended four times, but still — on schedule."

---

## 3. The Honest Diagram Caption

**Structure:** A slide title or caption that says what the diagram actually means, not what it's supposed to mean.

**Template:** Put the official title on the slide; deliver the honest version verbally as an aside.

**Worked examples:**

> *(Slide: "Current Architecture")*
> "We call this our current architecture. Technically, 'architecture' implies intent."

> *(Slide: "Simplified System Overview")*
> "This is the simplified version. The full version requires a support group."

> *(Slide: "Target State")*
> "This is where we're going. This is also what we showed stakeholders eighteen months ago. Progress is directional."

---

## 4. The Escalating List

**Structure:** Start with credible items. Each entry gets progressively more honest/absurd.

**Template:**
> "Our on-call runbook covers: [reasonable], [reasonable], [honest], [painfully honest]."

**Worked examples:**

> "Step 1: Check the dashboards. Step 2: Review recent deployments. Step 3: Ask yourself why you are on-call. Step 4: Call someone who knows what they're doing."

> "The system has four failure modes: network partition, database overload, memory leak, and me."

Tip: make yourself the final item. Naming a colleague ("…and Kevin") gets the laugh at their expense — see What to Avoid in the skill.

> "We've identified three root causes: misconfigured timeouts, cascading retries, and a deployment on a Thursday that everyone agreed not to do and then did."

---

## 5. The Callback Setup

**Structure:** Plant an image, phrase, or prop early. Return to it later when the talk has given it new meaning.

**Setup instructions:**

- Plant the callback in the first 10 minutes — naturally, without telegraphing it.
- Return 20–40 minutes later. The longer the gap, the bigger the laugh.
- The callback image/phrase should be tied to a key concept so the laugh reinforces the lesson.

**Example setup (slide 3):**
> "Here's how we felt on day one of the migration." *(shows optimistic stock photo of team celebrating)*

**Example callback (slide 22, after describing the chaos):**
> *(returns to same photo, now with a dumpster fire layered on top)*

---

## 6. The Misdirect Definition

**Structure:** Begin defining a term seriously. The definition turns out to mean something unexpected.

**Template:**
> "[Technical term]: [technical-sounding description]... [honest punchline]."

**Worked examples:**

> "Eventual consistency: a guarantee that your data will be correct. We don't specify when."

> "Technical debt: an investment in speed today, repaid with interest by whoever owns the system in two years — which, statistically, is also you."

> "Zero downtime deployment: a deployment completed without downtime, excluding the 40 minutes of 'elevated error rates' that don't officially count as downtime."

---

## 7. The Self-Deprecating War Story

**Structure:** Tell a real (or realistic) mistake story where you are the protagonist and the butt of the joke.

**Template:**

1. Establish your past confidence ("I was certain...")
2. Describe the action you took
3. Describe the immediate, humbling result
4. Extract the lesson you now live by

**Worked example:**

> "Early in my career I was absolutely certain I understood caching invalidation. I told my tech lead it was simple. He had that look — the one you give someone right before their lesson. I deployed on a Friday. By Monday morning, every user was seeing data from six months ago. I now have a personal policy: I don't use the word 'simple,' and I don't deploy on Fridays. Both policies were adopted simultaneously."

---

## 8. The Honest Sprint Review

**Structure:** Mirror the official sprint review language against what actually happened.

**Template:**
> "What we said we'd deliver: [optimistic scope]. What we delivered: [honest scope]. What we learned: [lesson that sounds better than 'we were wrong']."

**Worked example:**

> "Sprint goal: complete the auth module, migrate the user service, and add monitoring. What we delivered: the auth module. What we learned: we are consistently optimistic, and the auth module was harder than the auth module looked."

---

## Recovery Lines (For When a Joke Doesn't Land)

Have one of these ready. Deliver it dry, without panic:

- "That one plays better at conferences where it's late and everyone's had a drink."
- "Moving on — that joke will be funnier in the retrospective."
- *(beat)* "I'll workshop that one."
- "In my head, that was hilarious."
- "My test audience laughed. They were also being paid to attend."

The rule: say one recovery line, then move on immediately. Never apologize and never explain.
