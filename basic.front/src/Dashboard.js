export default function Dashboard() {
  return (
    <div className="container-xl">
      <div className="page-header d-print-none">
        <div className="row align-items-center">
          <div className="col">
            <div className="page-pretitle">Overview</div>
            <h2 className="page-title">Dashboard</h2>
          </div>

          {/*
                    <div className="col-auto ms-auto d-print-none">
                        <div className="btn-list">
                            <span className="d-none d-sm-inline">
                                <a href="#" className="btn btn-white">
                                    New view
                                </a>
                            </span>
                            <a href="#" className="btn btn-primary d-none d-sm-inline-block" data-bs-toggle="modal" data-bs-target="#modal-report">
                                <IconPlus /> Create new report
                            </a>
                            <a href="#" className="btn btn-primary d-sm-none btn-icon" data-bs-toggle="modal" data-bs-target="#modal-report" aria-label="Create new report">
                                <IconPlus />
                            </a>
                        </div>
                    </div>
                    */}
        </div>
      </div>
      <div className="page-body"></div>
    </div>
  );
}
