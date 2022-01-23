import MobilePageTitle from "../Generic/MobilePageTitle";

export function Dashboard() {
  return (
    <>
      <MobilePageTitle>
        <div className="navbar-brand">Dashboard</div>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="page-header d-print-none d-none d-lg-block">
          <div className="row align-items-center">
            <div className="col">
              <div className="page-pretitle">Overview</div>
              <h2 className="page-title">Dashboard</h2>
            </div>
          </div>
        </div>
        <div className="page-body"></div>
      </div>
    </>
  );
}
