import LayoutUser from "./LayoutUser";

function LayoutHeader() {
    return (
        <header className="navbar navbar-expand-md navbar-light d-none d-lg-flex d-print-none">
            <div className="container-xl">
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar-menu">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="navbar-nav flex-row order-md-last">
                    <LayoutUser />
                </div>
                <div className="collapse navbar-collapse" id="navbar-menu">
                    <div>
                    </div>
                </div>
            </div>
        </header>
    );
}

export default LayoutHeader;
