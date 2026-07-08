# GitHub Copilot – Pull Request Review Instructions

These instructions govern how GitHub Copilot reviews pull requests that touch
localization resource files (`Strings.*.xaml`).

---

## Scope

These rules apply to any PR that modifies one or more of the following files:

- `Strings.en-US.xaml` — the **source / reference** locale file
- `Strings.*.xaml` — any **translated** locale file (e.g. `Strings.fr-FR.xaml`,
  `Strings.de-DE.xaml`, `Strings.ja-JP.xaml`, etc.)

---

## Rule 1 — Reviewing `Strings.en-US.xaml` (Source Locale)

> **`Strings.en-US.xaml` is the single source of truth for all string keys.**

When this file is the subject of review, Copilot **must**:

- ✅ Check that every `<sys:String>` entry has a non-empty `x:Key` attribute and a
  non-empty child element.
- ✅ Flag duplicate `x:Key` attributes within the same file.
- ✅ Flag malformed XML syntax (unclosed tags, invalid characters, encoding
  issues).
- ✅ Suggest improvements to the English copy only if the existing value is
  clearly incorrect or misleading.
- ✅ Flag missing change log entry for the current change. The change log is located at the end of the file. 

When this file is the subject of review, Copilot **must not**:

- ❌ Suggest adding new keys to any translated `Strings.*.xaml` file.
- ❌ Report that translated files are "missing" the strings being added or
  changed — translation updates are handled separately by the localization
  pipeline.
- ❌ Open or reference any translated file as part of this review.

---

## Rule 2 — Reviewing Translated Files (`Strings.*.xaml`)

> **Translated files must stay structurally in sync with `Strings.en-US.xaml`.**

When any `Strings.*.xaml` file (other than `Strings.en-US.xaml`) is the subject
of review, Copilot **must**:

- ✅ **Compare keys against `Strings.en-US.xaml`** and report:
  - Keys present in `Strings.en-US.xaml` but **missing** from this file.
  - Keys present in this file but **absent** from `Strings.en-US.xaml`
    (orphaned / stale keys).
- ✅ Verify that every key's `x:Key` attribute exactly matches the corresponding
  `x:Key` in `Strings.en-US.xaml` (case-sensitive).
- ✅ Flag any `<value>` element that is empty or contains only whitespace —
  untranslated strings should be surfaced for attention.
- ✅ Flag malformed XML syntax (unclosed tags, invalid characters, wrong
  encoding declaration, BOM issues).
- ✅ Flag any structural differences from `Strings.en-US.xaml`, such as missing
  or extra XML attributes  (e.g. `xml:space="preserve"`).

When any translated file is the subject of review, Copilot **must not**:

- ❌ Suggest or rewrite the translated text itself — translation quality is
  owned by human translators or a localization service.
- ❌ Flag a translated value as "incorrect" solely because it differs from the
  English value (that is expected).
- ❌ Add, remove, or reorder keys to make the file match `Strings.en-US.xaml`
  automatically — only report discrepancies.

---

## Key Matching Reference

All comparisons between a translated file and `Strings.en-US.xaml` must be
**exact and case-sensitive** on the `name` attribute. Example:

```xml
<!-- Strings.en-US.xaml -->
<sys:String x:Key="About_CommitID">Commit ID</sys:String>

<!-- Strings.es-ES.xaml — CORRECT -->
<sys:String x:Key="About_CommitID">ID de commit</sys:String>

<!-- Strings.es-ES.xaml — FLAG: key casing mismatch -->
<sys:String x:Key="About_commitID">ID de commit</sys:String>
```

---

## Summary Table

| File reviewed          | Compare to `en-US`? | Report missing keys in translations? | Suggest translation edits? |
|------------------------|:-------------------:|:------------------------------------:|:--------------------------:|
| `Strings.en-US.xaml`  | N/A                 | ❌ No                                | ❌ No                      |
| `Strings.*.xaml`      | ✅ Yes              | ✅ Yes (in the reviewed file only)   | ❌ No                      |

---

## General XML Hygiene (All Files)

Regardless of which file is being reviewed, always flag:

1. Any non-UTF-8 characters or invalid XML escape sequences
2. Trailing whitespace inside `<value>` elements that could affect UI rendering
