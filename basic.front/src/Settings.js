import { Link } from "react-router-dom";
import MobilePageTitle from "./Generic/MobilePageTitle";

export default function Settings() {
  return (
    <>
      <MobilePageTitle>
        <div className="navbar-brand">Settings</div>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="page-header d-print-none d-none d-lg-block">
          <div className="row align-items-center">
            <div className="col">
              <div className="page-pretitle">Administration</div>
              <h2 className="page-title">Settings</h2>
            </div>
          </div>
        </div>
        <div className="page-body">
          <h3 className="p-1 mb-3 border-bottom border-primary">People</h3>
          <div className="row">
            <div className="col-12 col-lg-6 col-xl-4">
              <div className="card">
                <div className="card-body">
                  <h4 className="card-title">Event categories</h4>
                  <p className="card-text">
                    Manage the various event categories used when booking time (holidays, travel, sickness...).
                  </p>
                  <Link to="event-categories" className="btn btn-outline-primary">
                    More...
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
