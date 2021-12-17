import { IconMoon, IconSun } from "@tabler/icons";
import { Link } from "react-router-dom";

function LayoutUser() {
    document.body.className = localStorage.getItem('theme') || 'theme-light';
    function enableTheme(theme) {
        document.body.className = theme;
        localStorage.setItem('theme', theme);
    }

    return (
        <>
            <a href="#" className="nav-link px-0 hide-theme-dark" title="Enable dark mode"
                onClick={() => enableTheme('theme-dark')}>
                <IconMoon />
            </a>
            <a href="#" className="nav-link px-0 hide-theme-light" title="Enable light mode"
                onClick={() => enableTheme('theme-light')}>
                <IconSun />
            </a>
            <div className="nav-item dropdown">
                <a href="#" className="nav-link d-flex lh-1 text-reset p-0" data-bs-toggle="dropdown" aria-label="Open user menu">
                    <span className="avatar avatar-sm" style={{ backgroundImage: "url(/logo192.png)" }}></span>
                    <div className="ps-2">
                        <div>Ano Nymous</div>
                        <div className="mt-1 small text-muted">UX Designer</div>
                    </div>
                </a>
                <div className="dropdown-menu dropdown-menu-end dropdown-menu-arrow">
                    <Link to="/account" className="dropdown-item">Profile &amp; account</Link>
                    <a href="/account/logout" className="dropdown-item">Logout</a>
                </div>
            </div>
        </>
    );
}

export default LayoutUser;
