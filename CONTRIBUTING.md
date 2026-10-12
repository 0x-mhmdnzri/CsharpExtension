# مشارکت و Git Flow

این repository از Gitflow کلاسیک برای توسعه نسخه‌دار استفاده می‌کند.

## شاخه‌ها

- `main`: فقط نسخه‌های release شده و hotfixهای منتشرشده
- `develop`: شاخه integration نسخه بعدی
- `feature/<scope>-<description>`: قابلیت جدید از `develop`
- `release/<version>`: تثبیت نسخه از `develop`
- `hotfix/<version>`: اصلاح فوری از آخرین commit روی `main`

## قواعد تغییر

- مستقیم روی `main` و `develop` commit نکنید.
- هر commit باید build موفق داشته باشد.
- تغییرها کوچک، اتمیک و قابل review باشند.
- branch خصوصی قبل از review با `rebase` روی base به‌روز شود.
- روی branch مشترک rebase یا force push انجام نشود.
- merge release و hotfix با `--no-ff` انجام می‌شود تا مرز release در تاریخچه حفظ شود.

## نام‌گذاری commit

از Conventional Commits استفاده می‌کنیم:

```text
feat(server): add document analysis coordinator
fix(graph): reject stale document versions
docs(flow): define release branch policy
test(rules): cover boxing detection corpus
```

subject باید امری، حداکثر 72 کاراکتر و بدون نقطه پایانی باشد.

## نسخه‌گذاری

پروژه از Semantic Versioning استفاده می‌کند. تا پیش از نسخه پایدار، نسخه‌ها `0.x.y` هستند:

- قابلیت جدید: افزایش minor
- اصلاح سازگار: افزایش patch
- تغییر breaking در نسخه 0.x: افزایش minor و ثبت migration note

تگ‌ها immutable هستند و با الگوی `vMAJOR.MINOR.PATCH` یا pre-release متناظر ساخته می‌شوند.

## جریان feature

```powershell
git switch develop
git pull --ff-only
git switch -c feature/<scope>-<description>
# commitهای اتمیک و build/test
git fetch --prune
git rebase develop
git push -u origin feature/<scope>-<description>
```

## جریان release

```powershell
git switch develop
git switch -c release/0.1.0
# فقط fix، docs و آماده‌سازی release
git switch main
git merge --no-ff release/0.1.0 -m "release: 0.1.0"
git tag -a v0.1.0 -m "Release 0.1.0"
git switch develop
git merge --no-ff release/0.1.0 -m "merge: release 0.1.0 back to develop"
```

## جریان hotfix

```powershell
git switch main
git switch -c hotfix/0.1.1
# fix و test
git switch main
git merge --no-ff hotfix/0.1.1 -m "hotfix: 0.1.1"
git tag -a v0.1.1 -m "Hotfix 0.1.1"
git switch develop
git merge --no-ff hotfix/0.1.1 -m "merge: hotfix 0.1.1 back to develop"
```

