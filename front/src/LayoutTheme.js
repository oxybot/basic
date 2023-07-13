import { IconMoon, IconSun } from "@tabler/icons-react";
import clsx from "clsx";

export default function LayoutTheme({ className = "btn nav-link" }) {
  // Initialize the page with the current theme
  document.body.setAttribute("data-bs-theme", localStorage.getItem("theme") || "light");
  document.body.className = "theme-" + (localStorage.getItem("theme") || "light");

  function enableTheme(theme) {
    document.body.setAttribute("data-bs-theme", theme);
    document.body.className = "theme-" + theme;
    localStorage.setItem("theme", theme);
  }

  return (
    <>
      <button
        type="button"
        className={clsx(className, "px-0 btn-icon hide-theme-dark")}
        title="Enable dark mode"
        onClick={() => enableTheme("dark")}
      >
        <IconMoon />
      </button>
      <button
        type="button"
        className={clsx(className, "px-0 btn-icon hide-theme-light")}
        title="Enable light mode"
        onClick={() => enableTheme("light")}
      >
        <IconSun />
      </button>
    </>
  );
}
