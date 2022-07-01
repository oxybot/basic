import { IconMoon, IconSun } from "@tabler/icons";
import clsx from "clsx";

export default function LayoutTheme({ className = "btn nav-link" }) {
  // Initialize the page with the current theme
  document.body.className = localStorage.getItem("theme") || "theme-light";
  
  function enableTheme(theme) {
    document.body.className = theme;
    localStorage.setItem("theme", theme);
  }

  return (
    <>
      <button
        type="button"
        className={clsx(className, "px-0 btn-icon hide-theme-dark")}
        title="Enable dark mode"
        onClick={() => enableTheme("theme-dark")}
      >
        <IconMoon />
      </button>
      <button
        type="button"
        className={clsx(className, "px-0 btn-icon hide-theme-light")}
        title="Enable light mode"
        onClick={() => enableTheme("theme-light")}
      >
        <IconSun />
      </button>
    </>
  );
}
