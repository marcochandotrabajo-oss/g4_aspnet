(() => {
    const toggle = document.querySelector("[data-theme-toggle]");
    if (!toggle) {
        return;
    }

    const updateLabel = () => {
        const isDark = document.documentElement.getAttribute("data-bs-theme") === "dark";
        toggle.querySelector("[data-theme-label]").textContent = isDark ? "Claro" : "Oscuro";
        toggle.querySelector("[data-theme-sr-label]").textContent = isDark
            ? "Cambiar a tema claro"
            : "Cambiar a tema oscuro";
        toggle.setAttribute("aria-pressed", String(isDark));
    };

    toggle.addEventListener("click", () => {
        const nextTheme = document.documentElement.getAttribute("data-bs-theme") === "dark"
            ? "light"
            : "dark";
        document.documentElement.setAttribute("data-bs-theme", nextTheme);
        localStorage.setItem("g4-theme", nextTheme);
        updateLabel();
    });

    updateLabel();
})();
