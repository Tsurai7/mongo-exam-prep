# MongoDB Associate Developer Exam (C#) — 14-Day Study Plan

Target: pass the **MongoDB Associate Developer Exam, C# path** with a real understanding of MongoDB, not just exam cramming.

- **Pace:** ~2 hours/day, 14 days (~28 hours total). Checkpoint on **Day 7** — if you're ahead of schedule, compress Week 2.
- **Background assumed:** 2 years of hands-on MongoDB experience. Focus areas (self-identified weak spots): Aggregation, Indexes & performance, .NET Driver API, Transactions & Data Modeling — all get dedicated days.
- **Practice environment:** this repo — `mongo:7` via `make docker` (port **52601**), .NET 8 console app with `MongoDB.Driver` 3.8.1.

---

## 1. Exam facts (verified July 2026)

| Fact | Value |
|---|---|
| Format | 53 multiple-choice questions |
| Duration | 75 minutes (~85 sec/question) |
| Price | **$150**, but **50% off ($75) after completing the official Learning Path** |
| Delivery | Online, proctored by **ProctorU** |
| Language | English (extra time available on request via certification@mongodb.com) |
| Passing score | Not published (statistically determined); no per-domain minimum |
| Retakes | Unlimited, **15-day wait** between attempts, full price each time |
| Prerequisites | None |
| Booking | [learn.mongodb.com — Associate Developer Exam](https://learn.mongodb.com/pages/mongodb-associate-developer-exam) → Register Now |

> **Booking strategy:** don't book yet. Finish the Learning Path first (Days 1–11) to claim the 50% discount, then book the exam for Day 14 or shortly after, once the Day 13 mock run confirms you're ready.

### Exam domains and weights

Weights corroborated by multiple independent sources (MongoDB gates the official study guide behind a free account — verify there after enrolling):

| # | Domain | Weight | Plan coverage |
|---|---|---|---|
| 1 | MongoDB Overview & the Document Model | 8% | Day 1 |
| 2 | **CRUD** (queries, updates, deletes, **aggregation**, shaping results) | **51%** | Days 2–6 |
| 3 | Indexes | 17% | Days 8–9 |
| 4 | Data Modeling | 4% | Day 10 |
| 5 | Tools & Tooling | 2% | Day 9 |
| 6 | **Drivers (C#-specific section)** | **18%** | Days 11–12 (+ C# practice every day) |

Two takeaways: **more than half the exam is CRUD + aggregation syntax** — mostly in mongosh/shell form; and roughly 1 in 5 questions is C# driver code. Sections 1–5 are shared across all languages; only Section 6 is C#-specific.

### Official resources (all free)

- **[MongoDB C# Developer Path](https://learn.mongodb.com/learning-paths/using-mongodb-with-c-sharp)** — the official Learning Path (~15.5 h required units). Completing it earns the 50% exam discount. The plan below maps each day to its units.
- **[Associate Developer C# Practice Questions](https://learn.mongodb.com/courses/associate-developer-c-sharp-practice-questions)** — official, exam-grade questions in real exam format (~1 h). Used on Days 7 and 13.
- **Associate Developer Exam Study Guide** — on learn.mongodb.com after signing in; lists every numbered objective. Skim it on Day 1 and use it as your checklist.

---

## 2. Daily structure (2 hours)

Every day follows the same rhythm:

1. **~60 min — theory:** the mapped MongoDB University unit(s), watched at 1.25–1.5× since much will be familiar.
2. **~45 min — hands-on:** drills in mongosh and/or C# in this repo. Create one file per day (`Day02_QueryOperators.cs`, …) so you build a personal reference.
3. **~15 min — recap:** add gotchas you hit to `NOTES.md` (your growing cheat sheet), and re-answer yesterday's "self-check" questions from memory.

---

## 3. Week 1 — Foundations + CRUD + Aggregation (the 51%)

### Day 1 — Orientation, Document Model, environment

**Theory:** enroll in the [C# Developer Path](https://learn.mongodb.com/learning-paths/using-mongodb-with-c-sharp); units *Intro to MongoDB*, *MongoDB and the Document Model*, *Getting Started with Atlas*. Skim the official Study Guide objectives end-to-end so you know the target.

**Key concepts:**
- BSON vs JSON: extra types — `ObjectId`, `Date`, `Decimal128`, `Int32`/`Int64`, `Binary`. Type comparison order matters in sorts.
- `ObjectId` anatomy (timestamp + random + counter) — why it's roughly sortable by creation time.
- Documents, collections, databases; flexible schema; `_id` immutability.
- Atlas basics: cluster tiers, sample datasets (`sample_mflix`, `sample_training` — exam questions often use these shapes).

**Hands-on:**
- `make docker`, connect with `mongosh mongodb://localhost:52601`.
- Also create a **free Atlas M0 cluster and load the sample datasets** — the Learning Path labs use Atlas, and it doubles as tooling practice.
- From C#: insert documents with mixed BSON types; read them back as `BsonDocument` and inspect actual types.

**Self-check:** How do `Int32`, `Int64`, and `Double` compare in a sort? Can two documents in one collection have different fields? What happens if you insert a document without `_id`?

---

### Day 2 — CRUD: Insert & Find, query operators

**Theory:** units *Connecting via mongosh*, *CRUD: Insert and Find Documents*.

**Key concepts:**
- `insertOne` / `insertMany` (**`ordered: false`** behavior on duplicate-key errors — classic exam question).
- Comparison: `$eq, $ne, $gt, $gte, $lt, $lte, $in, $nin`.
- Logical: `$and, $or, $not, $nor` — and when implicit AND is enough.
- Element: `$exists`, `$type`.
- Arrays: matching a scalar inside an array, `$all`, `$size`, and **`$elemMatch` vs dot-notation** (multiple conditions on the *same* array element vs *any* elements).
- Nested documents: dot notation, and the trap that `{ field: {a: 1, b: 2} }` requires exact match including field order.

**Hands-on (mongosh, against sample data):** 10–15 query drills; deliberately write one `$elemMatch` query and the equivalent-looking dot-notation query and compare results.

**Self-check:** `find({ scores: { $gt: 80, $lt: 90 } })` vs `find({ scores: { $elemMatch: { $gt: 80, $lt: 90 } } })` on array fields — what differs? What does `insertMany` return after a mid-batch failure with `ordered: true`?

---

### Day 3 — CRUD: Update, Replace, Delete

**Theory:** unit *CRUD: Replace and Delete Documents*.

**Key concepts:**
- `updateOne` / `updateMany` vs `replaceOne` — replace swaps the whole document (except `_id`).
- Field update operators: `$set, $unset, $inc, $mul, $rename, $min, $max, $setOnInsert`.
- Array update operators: `$push, $pop, $pull, $addToSet`, with modifiers `$each, $slice, $sort, $position`; positional `$`, all-positional `$[]`, filtered `$[identifier]` + `arrayFilters`.
- **Upsert** semantics: what document gets created (query fields + update operators + `$setOnInsert`).
- `findOneAndUpdate` / `findOneAndReplace` / `findOneAndDelete` — atomic read-modify-write; `returnDocument: "before" | "after"` (C#: `ReturnDocument.After`).
- `deleteOne` / `deleteMany`; `drop()` vs `deleteMany({})`.

**Hands-on:** in mongosh — upsert drills (predict the created document before running); in C# — the same updates with `Builders<T>.Update`.

**Self-check:** After `updateOne({ qty: { $lt: 5 } }, { $set: { flag: true } }, { upsert: true })` matches nothing — what exactly is inserted? How do you increment `items.$[e].qty` only where `e.qty > 0`?

---

### Day 4 — CRUD: Shaping results (cursors, projection, counting)

**Theory:** unit *CRUD: Modifying Query Results*.

**Key concepts:**
- `sort()`, `limit()`, `skip()` — and that MongoDB applies **sort → skip → limit** regardless of the order you chain them in the shell.
- Projection: inclusion vs exclusion (can't mix, except excluding `_id`); projecting array elements with `$slice`.
- `countDocuments()` vs `estimatedDocumentCount()` (accuracy vs speed, filter support).
- `distinct()`.
- Cursor behavior: batching, iteration in mongosh vs drivers.

**Hands-on:** pagination drills (page 3, 10 per page, sorted by two fields); projection drills including nested fields. In C#: `Find(...).Sort(...).Skip(...).Limit(...).Project(...)`.

**Self-check:** Why is `skip`-based pagination slow on page 10,000 and what's the range-query alternative? What does `countDocuments({})` do differently from `estimatedDocumentCount()`?

---

### Day 5 — Aggregation I: core pipeline

**Theory:** unit *MongoDB Aggregation*.

**Key concepts:**
- Pipeline mental model: documents flow stage → stage; **stage order matters** for both correctness and performance (`$match` and `$sort` early → can use indexes).
- `$match`, `$project`, `$set`/`$addFields`, `$unset`, `$sort`, `$limit`, `$skip`, `$count`.
- `$group`: `_id` expression, accumulators — `$sum, $avg, $min, $max, $push, $addToSet, $first, $last, $count`.
- Expression syntax: `"$fieldName"` references, `$cond`, `$ifNull`, arithmetic/string/date operators.
- `$out` and `$merge` (writing results; `$out` replaces the target collection, must be the last stage).

**Hands-on (mongosh on `sample_mflix` / `sample_training`):** build 6–8 pipelines of increasing complexity — e.g., average movie rating per genre, top-5 by count; group per year, then per (year, rated) pair.

**Self-check:** In `$group`, what does `_id: null` do? Difference between `$push` and `$addToSet`? Why put `$match` before `$group` rather than after?

---

### Day 6 — Aggregation II: $lookup, $unwind, and traps

**Theory:** finish the aggregation unit; unit *MongoDB Aggregation with C#* (first pass).

**Key concepts:**
- `$unwind`: one doc per array element; `preserveNullAndEmptyArrays` (what happens to docs with missing/empty arrays without it — they **disappear**).
- `$lookup` (equality form: `from`, `localField`, `foreignField`, `as`) — result is always an array; the common `$lookup` → `$unwind` → `$project` combo.
- `$facet`, `$bucket` (recognition level), `$sortByCount`.
- Memory limits: 100 MB per stage, `allowDiskUse`.
- Aggregation vs find: when a simple query suffices.
- C# side: `Aggregate().Match(...).Group(...)` fluent API and `PipelineDefinition`, `AppendStage<T>` for raw stages.

**Hands-on:** join two sample collections with `$lookup`; unwind an array field and re-group to compute per-element stats. Redo two of Day 5's pipelines in C#.

**Self-check:** What's in the `as` field when `$lookup` finds no matches? What happens to a document with an empty array after a bare `$unwind`?

---

### Day 7 — CHECKPOINT: official practice questions + Week 1 review

1. **Take the [official C# practice questions](https://learn.mongodb.com/courses/associate-developer-c-sharp-practice-questions) (~1 h), timed.** This is the real exam's question style.
2. Score yourself per domain. Review every wrong/uncertain answer against the docs (~45 min).
3. **Decide pace:**
   - **≥80% and Week 1 felt easy** → compress Week 2: merge Days 8+9 and 11+12, target the exam around Day 10–11.
   - **60–80%** → keep the 14-day plan as is.
   - **<60%** → likely weak on aggregation or operator details; add 2–3 buffer days and re-drill before moving on.

---

## 4. Week 2 — Indexes, Modeling, Transactions, C# Driver (the other 49%)

### Day 8 — Indexes I: fundamentals and the ESR rule

**Theory:** unit *MongoDB Indexes*.

**Key concepts:**
- Index types: single-field, **compound**, **multikey** (auto, when a field is an array; a compound index may contain **at most one** array field).
- **Compound index prefix rule:** an index on `{a: 1, b: 1, c: 1}` supports queries on `a`, `a+b`, `a+b+c` — not `b` or `b+c` alone.
- **ESR rule** for ordering compound index keys: **Equality → Sort → Range**.
- Sort direction: when `{a: 1, b: -1}` can satisfy `sort({a: -1, b: 1})` (reverse traversal) but not `sort({a: 1, b: 1})`.
- Unique, sparse, TTL, partial indexes (recognition level).
- `createIndex`, `dropIndex`, `getIndexes`; index build impact.

**Hands-on:** on a ~100k-doc collection (script the inserts in C#), create indexes and probe them with queries; break the prefix rule on purpose and observe.

**Self-check:** For `find({ status: "A", qty: { $gt: 10 } }).sort({ ts: -1 })` — ideal index and key order, per ESR? Can `{a: 1, b: 1}` support `find({b: 5})`?

---

### Day 9 — Indexes II: explain(), covered queries + Tools

**Theory:** finish the indexes unit; skim tooling docs.

**Key concepts:**
- `explain()` verbosity levels; reading the winning plan: **COLLSCAN vs IXSCAN**, `FETCH`, in-memory `SORT` (bad sign), `totalKeysExamined` / `totalDocsExamined` / `nReturned` ratios.
- **Covered queries:** index contains all queried + projected fields, `_id` excluded in projection → no `FETCH`.
- Tradeoffs: write amplification, index size; why not index everything.
- **Tools (2%):** mongosh basics, MongoDB Compass (visual explain, index tab), Atlas Data Explorer, `mongodump`/`mongorestore` vs `mongoexport`/`mongoimport` (BSON vs JSON — know which is which), MongoDB for VS Code.

**Hands-on:** run `explain("executionStats")` on Day 8's queries before/after adding indexes; construct one covered query and prove it (no `FETCH` stage, `totalDocsExamined: 0`). Open Compass against `localhost:52601` and use visual explain once.

**Self-check:** In explain output, what signals an index that filters well but doesn't cover the sort? Which tool exports BSON: `mongodump` or `mongoexport`?

---

### Day 10 — Data Modeling + Transactions

**Theory:** unit *MongoDB Transactions*; data-modeling sections of the study guide.

**Key concepts — modeling (4%, conceptual questions):**
- **Embed vs reference** — the golden rule: *data that is accessed together should be stored together*.
- One-to-few → embed; one-to-many → embed array or child-references; one-to-squillions → reference from the many side.
- Antipatterns: unbounded arrays, massive documents (16 MB limit), excessive `$lookup`.
- Schema patterns at recognition level: computed, extended reference, bucket.

**Key concepts — transactions:**
- Single-document operations are **already atomic** — the most important fact; multi-document transactions are for the rare cross-document invariant.
- Sessions → `StartTransaction` → commit/abort; transactions have a 60-second default lifetime and performance cost.
- C# API: `client.StartSession()`, `session.WithTransaction(...)` callback (preferred — handles retries) vs manual `StartTransaction`/`CommitTransaction`; **every operation inside must pass the `session` handle**.

**Hands-on (C#):** model a small e-commerce pair (orders + products) both embedded and referenced; implement a balance transfer between two account documents with `WithTransaction`, make it fail mid-way, verify rollback.

**Self-check:** When is a multi-document transaction actually necessary vs a schema redesign? What happens to writes in an aborted transaction? Why are unbounded arrays a problem for both storage and indexes?

---

### Day 11 — C# Driver deep dive I: connection, mapping, Builders

**Theory:** units *Connecting to MongoDB in C#*, *MongoDB CRUD Operations in C#* (part 1).

**Key concepts:**
- `MongoClient` is **thread-safe and should be a singleton**; connection string options; `GetDatabase`/`GetCollection` are cheap.
- POCO mapping: `[BsonId]`, `[BsonElement("name")]`, `[BsonIgnore]`, `[BsonIgnoreExtraElements]`, `[BsonRepresentation(BsonType.ObjectId)]` (string ↔ ObjectId — very common in questions).
- `BsonDocument` vs typed `TDocument` collections — when each is used.
- **`Builders<T>`** — the exam's favorite C# construct: `Builders<T>.Filter` (`Eq, Gt, In, And, Or, ElemMatch, Exists`), `.Update` (`Set, Inc, Push, AddToSet`), `.Sort`, `.Projection`; the `&`/`|` operator shortcuts on filters.
- Sync vs async pairs (`Find`/`FindAsync`, `InsertOne`/`InsertOneAsync`); materializers: `ToList`, `FirstOrDefault`, `ToCursor`, `AnyAsync`.

**Hands-on (this repo):** expand `MyDoc` into a realistic POCO with attributes; write one method per Builders category. Practice **reading** driver snippets and predicting output — that's the exam skill.

**Self-check:** Why must `MongoClient` not be created per request? What does `[BsonRepresentation(BsonType.ObjectId)]` on a `string` property do on insert and on read? What does `Builders<T>.Filter.ElemMatch` translate to in MQL?

---

### Day 12 — C# Driver deep dive II: LINQ, aggregation, results

**Theory:** unit *MongoDB Aggregation with C#* (second pass); *CRUD in C#* (part 2).

**Key concepts:**
- LINQ provider: `AsQueryable()`, `Where/Select/OrderBy/GroupBy` translated to aggregation pipelines; what is and isn't translatable.
- Fluent aggregation: `collection.Aggregate().Match().Group().Sort().Limit()`, `Lookup`, `Unwind`, `Project`.
- Result/return types: `InsertOne*` returns void (throws on error) vs `UpdateResult` (`MatchedCount`, `ModifiedCount`, `UpsertedId`) vs `DeleteResult` (`DeletedCount`) — know which operation returns what, and that **`ModifiedCount` can be 0 while `MatchedCount` is 1** (no-op update).
- `ReplaceOptions`/`UpdateOptions` `{ IsUpsert = true }`; `FindOneAndUpdateOptions` `ReturnDocument`.
- Exceptions: `MongoWriteException` (duplicate key), `MongoBulkWriteException` for `InsertMany`.

**Hands-on:** redo Day 5–6 pipelines three ways where possible: raw `BsonDocument` stages, fluent API, LINQ. Trigger a duplicate-key error and inspect the exception.

**Self-check:** What does `UpdateResult.ModifiedCount == 0, MatchedCount == 1` mean? Which C# call is equivalent to `db.coll.find().sort({x:1}).limit(5)`?

---

### Day 13 — Full mock + weak-area drill

1. **Retake the official practice questions, timed, exam conditions** (75-min mindset: ~85 sec/question, flag and move on).
2. Grade per domain; drill the weakest domain for ~40 min with docs + hands-on.
3. Re-read your entire `NOTES.md` cheat sheet.
4. If not yet booked: **complete any remaining Learning Path units** (this is what triggers the 50% discount), then **book the exam via ProctorU** for tomorrow or the next day. Check the tech requirements (webcam, room scan, government ID, closed browser).

---

### Day 14 — Light review + exam

- **No new material.** 30–40 min: cheat sheet, self-check questions from Days 1–12, skim the gotcha list below.
- Exam mechanics: 53 questions / 75 min; answer everything (no penalty for guessing); flag long aggregation-reading questions and return to them; you don't need per-domain passes, only the overall score.
- If you fail (it happens): you get a domain-level score report — that's your targeted study list for the retake after the 15-day wait.

---

## 5. Top gotchas (re-read before the exam)

1. `$elemMatch` = multiple conditions on the **same** array element; plain `{arr: {$gt: 5, $lt: 10}}` can be satisfied by **different** elements.
2. Exact subdocument match `{addr: {city: "X", zip: "Y"}}` is order-sensitive and requires no extra fields; dot notation is not.
3. Upsert inserts **query equality fields + update operator results + `$setOnInsert`**.
4. `replaceOne` removes fields not in the replacement; `updateOne` with `$set` doesn't.
5. Inclusion and exclusion projections can't mix — except `_id: 0`.
6. `sort → skip → limit` is the effective server order regardless of chaining order.
7. Compound index **prefix rule**; **ESR** (Equality, Sort, Range) for key ordering; a compound index allows at most **one array field**.
8. An index `{a: 1, b: -1}` serves `sort({a: -1, b: 1})` (reverse) but **not** `sort({a: 1, b: 1})`.
9. Covered query: all filter+projection fields in the index **and** `_id: 0` (unless `_id` is in the index).
10. `explain`: `COLLSCAN` = no index; `FETCH` = not covered; in-memory `SORT` stage = index didn't cover the sort.
11. Bare `$unwind` **drops** documents with missing/empty arrays; `$lookup`'s `as` is always an array (possibly empty).
12. `$out` replaces the whole target collection and must be last.
13. Aggregation stage memory limit 100 MB → `allowDiskUse`.
14. Single-document writes are atomic **without** transactions.
15. In a C# transaction, every operation must receive the `session` argument.
16. `MongoClient` = singleton; `ModifiedCount` 0 with `MatchedCount` 1 = matched but nothing changed; `insertMany`/`InsertMany` with `ordered: false` continues past errors.
17. `mongodump`/`mongorestore` = BSON (backup); `mongoexport`/`mongoimport` = JSON/CSV.
18. `countDocuments(filter)` is accurate and filterable; `estimatedDocumentCount()` is fast, metadata-based, no filter.

---

## 6. Progress tracker

| Day | Topic | Theory done | Hands-on done | Notes updated |
|---|---|---|---|---|
| 1 | Document Model + setup | ☐ | ☐ | ☐ |
| 2 | Insert & Find, operators | ☐ | ☐ | ☐ |
| 3 | Update, Replace, Delete | ☐ | ☐ | ☐ |
| 4 | Shaping results | ☐ | ☐ | ☐ |
| 5 | Aggregation core | ☐ | ☐ | ☐ |
| 6 | Aggregation advanced | ☐ | ☐ | ☐ |
| 7 | **Checkpoint: practice test** | ☐ | ☐ | ☐ |
| 8 | Indexes: ESR | ☐ | ☐ | ☐ |
| 9 | Indexes: explain + Tools | ☐ | ☐ | ☐ |
| 10 | Modeling + Transactions | ☐ | ☐ | ☐ |
| 11 | C# driver: Builders, mapping | ☐ | ☐ | ☐ |
| 12 | C# driver: LINQ, aggregation | ☐ | ☐ | ☐ |
| 13 | Mock exam + booking | ☐ | ☐ | ☐ |
| 14 | Review + **EXAM** | ☐ | ☐ | ☐ |

---

## 7. Links

- Exam page / booking: https://learn.mongodb.com/pages/mongodb-associate-developer-exam
- C# Learning Path: https://learn.mongodb.com/learning-paths/using-mongodb-with-c-sharp
- Official C# practice questions: https://learn.mongodb.com/courses/associate-developer-c-sharp-practice-questions
- MongoDB Manual (CRUD): https://www.mongodb.com/docs/manual/crud/
- Aggregation reference: https://www.mongodb.com/docs/manual/reference/operator/aggregation-pipeline/
- Indexes / ESR: https://www.mongodb.com/docs/manual/tutorial/equality-sort-range-guideline/
- C# driver docs: https://www.mongodb.com/docs/drivers/csharp/current/
- Community exam reports & threads: https://www.mongodb.com/community/forums/ (search "Associate Developer")
