import { IconDashboard, IconConfetti, IconNotebook, IconPackage } from "@tabler/icons";
import { NavLink } from "react-router-dom";
import LayoutTheme from "./LayoutTheme";

export default function LayoutMenu() {

  function closeMenu(event) {
    if (window.bootstrap) {
      var menu = window.bootstrap.Offcanvas.getInstance("#offcanvas-menu");
      menu && menu.hide();
    }
  }

  return (
    <aside className="navbar navbar-vertical navbar-light navbar-expand-lg navbar-hidden">
      <div className="container-fluid justify-content-start">
        <div
          id="offcanvas-menu"
          className="offcanvas offcanvas-start"
          tabIndex="-1"
        >
          <div className="offcanvas-header">
            <h5 className="offcanvas-title flex-fill">
              <div className="basic-logo">B</div>
            </h5>
            <LayoutTheme />
            <button
              type="button"
              className="btn-close text-reset"
              data-bs-dismiss="offcanvas"
              aria-label="Close"
            ></button>
          </div>
          <div className="offcanvas-body flex-column">
            <div className="d-flex flex-row justify-content-between d-none d-lg-flex">
              <h5 className="basic-title">
                <div className="basic-logo">B</div>
              </h5>
              <LayoutTheme />
            </div>
            {/* Profile */}
            <ul className="navbar-nav">
              <li className="nav-item dropdown">
                <button
                  type="button"
                  className="nav-link dropdown-toggle collapsed justify-content-start d-flex lh-1 text-reset"
                  data-bs-toggle="collapse"
                  data-bs-target="#menu-user"
                  aria-label="Open user menu"
                >
                  <span
                    className="avatar avatar-sm"
                    style={{ backgroundImage: "url(/logo192.png)" }}
                  ></span>
                  <div className="ps-2 flex-fill text-start">
                    <div>Ano Nymous</div>
                    <div className="mt-1 small text-muted">UX Designer</div>
                  </div>
                </button>
                <ul id="menu-user" className="navbar-nav collapse">
                  <li className="nav-item">
                    <NavLink
                      to="/account"
                      end
                      className="nav-link justify-content-start"
                      onClick={closeMenu}
                    >
                      Profile &amp; account
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink
                      to="/account/logout"
                      className="nav-link justify-content-start"
                      onClick={closeMenu}
                    >
                      Logout
                    </NavLink>
                  </li>
                </ul>
              </li>
            </ul>
            <hr className="my-2" />
            {/* Dashboard */}
            <ul className="navbar-nav">
              <li className="nav-item">
                <NavLink
                  className="nav-link justify-content-start"
                  to="/"
                  onClick={closeMenu}
                >
                  <span className="nav-link-icon">
                    <IconDashboard />
                  </span>
                  <span className="nav-link-title">Dashboard</span>
                </NavLink>
              </li>
            </ul>
            <hr className="my-2" />
            {/* Client */}
            <ul className="navbar-nav">
            <li className="nav-item">
                <NavLink
                  className="nav-link justify-content-start"
                  to="/clients"
                  onClick={closeMenu}
                >
                  <span className="nav-link-icon">
                    <IconConfetti />
                  </span>
                  <span className="nav-link-title">Clients</span>
                </NavLink>
              </li>
              <li className="nav-item">
                <NavLink
                  className="nav-link justify-content-start"
                  to="/products"
                  onClick={closeMenu}
                >
                  <span className="nav-link-icon">
                    <IconPackage />
                  </span>
                  <span className="nav-link-title">Products</span>
                </NavLink>
              </li>
              <li className="nav-item">
                <NavLink
                  className="nav-link justify-content-start"
                  to="/agreements"
                  onClick={closeMenu}
                >
                  <span className="nav-link-icon">
                    <IconNotebook />
                  </span>
                  <span className="nav-link-title">Agreements</span>
                </NavLink>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </aside>
  );
}
