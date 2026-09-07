# **CONTRIBUTING.md**

# Contributing to Capillume

Thank you for your interest in contributing to **Capillume**!  
We welcome improvements, bug fixes, documentation updates, and new features.
This guide explains how to contribute effectively and how to work with the project maintainers.

---

## 📌 How to Get Started

### 1. **Fork the repository**
Create your own fork of Capillume:

```
https://github.com/dasdebjyoti/Capillume
```

Clone your fork locally:

```bash
git clone https://github.com/<your-username>/Capillume.git
```

---

### 2. **Create a feature branch**
Use a descriptive branch name:

```bash
git checkout -b feature/your-feature-name
```

Examples:
- `feature/retention-policy`
- `fix/downscale-bounding-box`
- `docs/update-readme`

---

### 3. **Build the project**
Capillume requires:

- Windows 10 or later  
- .NET 10 SDK/runtime  
- Visual Studio 2022 or JetBrains Rider  

Build using:

```bash
dotnet restore
dotnet build
```

Run the app:

```bash
dotnet run --project .\Capillume.csproj
```

---

## 🧩 Coding Guidelines

### ✔ Follow existing code style
- Use C# conventions already present in the project  
- Keep UI code consistent with existing WinForms patterns  
- Keep settings classes simple, serializable, and JSON‑friendly  
- Use meaningful names for enums and settings fields  
- Avoid introducing new dependencies unless necessary

### ✔ Keep PRs focused
One PR = one feature or one fix.

### ✔ Add comments where needed
Especially for:
- Screenshot pipeline logic  
- Downscaling  
- Image processing  
- Retention cleanup  
- Settings serialization  

---

## 🧪 Testing Your Changes

Before submitting a PR:

- Build the project  
- Run the app  
- Test your feature manually  
- Verify that screenshots still save correctly  
- Check that settings load/save properly  
- Confirm no exceptions appear in the console  

---

## 🔄 Submitting a Pull Request

### 1. Push your branch to your fork:

```bash
git push origin feature/your-feature-name
```

### 2. Open a Pull Request on GitHub
Choose:

- **Base branch:** `main`  
- **Compare branch:** your feature branch  

### 3. PR Requirements
Your pull request should include:

- A clear description of the change  
- Screenshots if UI changes were made  
- Notes on testing  
- Any new settings or configuration fields  
- Any breaking changes (rare)  

### 4. Code Review
All PRs require at least **one approval** before merging.

Maintainers may request changes such as:

- Naming adjustments  
- UI consistency fixes  
- Code simplification  
- Documentation updates  

Please respond to feedback promptly.

---

## 🔐 Branch Protection Rules

The `main` branch is protected:

- Direct pushes are not allowed  
- All changes must come through pull requests  
- At least one approval is required  
- Force pushes and branch deletion are disabled  
- Administrators follow the same rules  

This ensures a clean, stable history.

---

## 🐛 Reporting Issues

If you find a bug or want to request a feature:

1. Go to the **Issues** tab  
2. Search existing issues  
3. If not found, open a new issue  
4. Provide:
   - Steps to reproduce  
   - Expected behavior  
   - Actual behavior  
   - Screenshots (if applicable)  

---

## 🌟 Good First Issues

If you're new to the project, look for issues labeled:

```
good first issue
help wanted
```

These are beginner‑friendly and well‑scoped.

---

## 📄 License

By contributing, you agree that your contributions will be licensed under the same license as the Capillume project.

---

## ❤️ Thank You

Your contributions help make Capillume better for everyone.  
We appreciate your time, effort, and interest in improving this project.

If you need help getting started, feel free to open an issue or ask questions in your pull request.

---

If you want, I can also generate:

- A **Pull Request Template**  
- An **Issue Template**  
- A **Code Style Guide**  
- A **Maintainer Guide**  

Just tell me what you want next.
