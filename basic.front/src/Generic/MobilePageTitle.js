export default function MobilePageTitle({ children }) {
  return (
    <aside className="navbar navbar-light d-lg-none sticky-top">
      <div className="container-fluid justify-content-start">
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="offcanvas"
          data-bs-target="#offcanvas-menu"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        {children}
      </div>
    </aside>
  );
}
