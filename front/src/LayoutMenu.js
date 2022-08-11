import {
  IconDashboard,
  IconConfetti,
  IconNotebook,
  IconPackage,
  IconFriends,
  IconSettings,
  IconCalendarEvent,
  IconChartArrows,
  IconSunset,
  IconCalendarTime,
  IconApiApp,
  IconBrandGithub,
} from "@tabler/icons";
import Cookies from "js-cookie";
import { useDispatch, useSelector } from "react-redux";
import { NavLink } from "react-router-dom";
import { authenticationState, useInRole, disconnect} from "./Authentication";
import LayoutMenuDemo from "./LayoutMenuDemo";
import LayoutTheme from "./LayoutTheme";

const rootApiUrl = process.env.REACT_APP_API_ROOT_URL || document.getElementById("apirooturl").innerHTML.trim();

export default function LayoutMenu() {
  const { user } = useSelector(authenticationState);
  const isInrole = useInRole();
  const dispatch = useDispatch();

  function closeMenu(event) {
    if (window.bootstrap) {
      var menu = window.bootstrap.Offcanvas.getInstance("#offcanvas-menu");
      menu && menu.hide();
    }
  }

  function logout() {
    closeMenu();
    dispatch(disconnect());
    Cookies.remove("access-token");
  }

  return (
    <aside className="navbar navbar-vertical navbar-light navbar-expand-lg navbar-hidden">
      <div className="container-fluid justify-content-start">
        <div id="offcanvas-menu" className="offcanvas offcanvas-start" tabIndex="-1">
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
                  <span className="avatar avatar-sm">
                    {user.avatar && <img src={`data:${user.avatar.mimeType};base64,${user.avatar.data}`} alt="" />}
                  </span>
                  <div className="ps-2 flex-fill text-start">
                    <div>{user.displayName}</div>
                    <div className="mt-1 small text-muted">{user.title}</div>
                  </div>
                </button>
                <ul id="menu-user" className="navbar-nav collapse">
                  <li className="nav-item">
                    <NavLink to="/my/profile" end className="nav-link justify-content-start" onClick={closeMenu}>
                      Profile &amp; account
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink to="/" className="nav-link justify-content-start" onClick={logout}>
                      Logout
                    </NavLink>
                  </li>
                </ul>
              </li>
              {user.userName === "demo" && <LayoutMenuDemo />}
            </ul>
            {/* Dashboard */}
            <hr className="my-2" />
            <ul className="navbar-nav">
              <li className="nav-item">
                <NavLink className="nav-link justify-content-start" to="/" onClick={closeMenu}>
                  <span className="nav-link-icon">
                    <IconDashboard />
                  </span>
                  <span className="nav-link-title">Dashboard</span>
                </NavLink>
              </li>
              <li className="nav-item">
                <NavLink className="nav-link justify-content-start" to="/calendar" onClick={closeMenu}>
                  <span className="nav-link-icon">
                    <IconCalendarEvent />
                  </span>
                  <span className="nav-link-title">Calendar</span>
                </NavLink>
              </li>
            </ul>
            {/* Client */}
            {isInrole("client", "client-ro") && (
              <>
                <hr className="my-2" />
                <ul className="navbar-nav">
                  <li className="nav-item">
                    <NavLink className="nav-link justify-content-start" to="/clients" onClick={closeMenu}>
                      <span className="nav-link-icon">
                        <IconConfetti />
                      </span>
                      <span className="nav-link-title">Clients</span>
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink className="nav-link justify-content-start" to="/products" onClick={closeMenu}>
                      <span className="nav-link-icon">
                        <IconPackage />
                      </span>
                      <span className="nav-link-title">Products</span>
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink className="nav-link justify-content-start" to="/agreements" onClick={closeMenu}>
                      <span className="nav-link-icon">
                        <IconNotebook />
                      </span>
                      <span className="nav-link-title">Agreements</span>
                    </NavLink>
                  </li>
                </ul>
              </>
            )}
            {/* User */}
            {isInrole("time", "time-ro", "user") && (
              <>
                <hr className="my-2" />
                <ul className="navbar-nav">
                  <li className="nav-item">
                    <NavLink className="nav-link justify-content-start" to="/users" onClick={closeMenu}>
                      <span className="nav-link-icon">
                        <IconFriends />
                      </span>
                      <span className="nav-link-title">People</span>
                    </NavLink>
                  </li>
                  {isInrole("time", "time-ro") && (
                    <>
                      <li className="nav-item">
                        <NavLink className="nav-link justify-content-start" to="/balances" onClick={closeMenu}>
                          <span className="nav-link-icon">
                            <IconChartArrows />
                          </span>
                          <span className="nav-link-title">Balances</span>
                        </NavLink>
                      </li>
                      <li className="nav-item">
                        <NavLink className="nav-link justify-content-start" to="/schedules" onClick={closeMenu}>
                          <span className="nav-link-icon">
                            <IconCalendarTime />
                          </span>
                          <span className="nav-link-title">Working Schedules</span>
                        </NavLink>
                      </li>
                      <li className="nav-item">
                        <NavLink className="nav-link justify-content-start" to="/events" onClick={closeMenu}>
                          <span className="nav-link-icon">
                            <IconSunset />
                          </span>
                          <span className="nav-link-title">Events</span>
                        </NavLink>
                      </li>
                    </>
                  )}
                </ul>
              </>
            )}
            {/* Settings */}
            {isInrole("time", "time-ro") && (
              <>
                <hr className="my-2" />
                <ul className="navbar-nav">
                  <li className="nav-item">
                    <NavLink className="nav-link justify-content-start" to="/settings" onClick={closeMenu}>
                      <span className="nav-link-icon">
                        <IconSettings />
                      </span>
                      <span className="nav-link-title">Settings</span>
                    </NavLink>
                  </li>
                </ul>
              </>
            )}
          </div>
          <div className="offcanvas-footer text-end">
            <a
              className="btn btn-icon btn-outline-secondary"
              target="_blank"
              rel="noreferrer"
              href={rootApiUrl}
            >
              <IconApiApp />
            </a>
            <a
              className="btn btn-icon btn-outline-secondary ms-3"
              target="_blank"
              rel="noreferrer"
              href="https://github.com/oxybot/basic"
            >
              <IconBrandGithub />
            </a>
          </div>
        </div>
      </div>
    </aside>
  );
}