import { IconArrowLeft } from "@tabler/icons";
import { Link } from "react-router-dom";

export default function MobilePageTitle({ back, children }) {
  return (
    <aside className="navbar navbar-light d-lg-none sticky-top">
      <div className="container-fluid justify-content-start">
        {!back && (
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="offcanvas"
            data-bs-target="#offcanvas-menu"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
        )}
        {back && (
          <Link to={back} role="button" className="navbar-toggler">
            <IconArrowLeft />
          </Link>
        )}
        {children}
      </div>
    </aside>
  );
}
