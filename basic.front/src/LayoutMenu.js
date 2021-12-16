import { IconDashboard, IconConfetti } from '@tabler/icons';
import { Link } from "react-router-dom";
import LayoutUser from "./LayoutUser";

function LayoutMenu() {
    return (
        <aside className="navbar navbar-vertical navbar-expand-lg">
            <div className="container-fluid">
                <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvas-menu">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <Link to="/" className="navbar-brand navbar-brand-autodark">
                    <img src="/logo192.png" width="110" height="32" alt="Basic" className="navbar-brand-image" />
                </Link>
                <div className="navbar-nav flex-row d-lg-none">
                    <LayoutUser />
                </div>
                <div id="offcanvas-menu" className="offcanvas offcanvas-start" tabIndex="-1">
                    <div className="offcanvas-header">
                        <h5 className="offcanvas-title">Menu</h5>
                        <button type="button" className="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                    </div>
                    <div className="offcanvas-body">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <Link className="nav-link justify-content-start" to="/">
                                    <span className="nav-link-icon">
                                        <IconDashboard />
                                    </span>
                                    <span className="nav-link-title">
                                        Dashboard
                                    </span>
                                </Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link justify-content-start" to="/clients">
                                    <span className="nav-link-icon">
                                        <IconConfetti />
                                    </span>
                                    <span className="nav-link-title">
                                        Clients
                                    </span>
                                </Link>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </aside>
    );
}

export default LayoutMenu;
