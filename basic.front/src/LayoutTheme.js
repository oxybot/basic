import { IconMoon, IconSun } from "@tabler/icons";

export default function LayoutTheme() {
  document.body.className = localStorage.getItem("theme") || "theme-light";
  function enableTheme(theme) {
    document.body.className = theme;
    localStorage.setItem("theme", theme);
  }

  return (
    <>
      <button
        type="button"
        className="nav-link px-0 btn btn-icon hide-theme-dark"
        title="Enable dark mode"
        onClick={() => enableTheme("theme-dark")}
      >
        <IconMoon />
      </button>
      <button
        type="button"
        className="nav-link px-0 btn btn-icon hide-theme-light"
        title="Enable light mode"
        onClick={() => enableTheme("theme-light")}
      >
        <IconSun />
      </button>
    </>
  );
}
