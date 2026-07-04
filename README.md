# mongo-exam-prep

Practice environment for the **MongoDB Associate Developer Exam (C# path)**.

- [STUDY_PLAN.md](STUDY_PLAN.md) — 14-day study plan: exam facts, day-by-day topics, gotcha list, progress tracker
- `mongo-exam-prep/` — .NET 8 console app for driver practice

## Setup

```bash
make docker        # start MongoDB 7 in Docker (localhost:52601)
make docker-down   # stop it
```

Connect:

```bash
mongosh mongodb://localhost:52601
```

Run the C# app:

```bash
dotnet run --project mongo-exam-prep
```
