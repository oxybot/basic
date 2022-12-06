import { Outlet } from "react-router-dom";
import LayoutMenu from "./LayoutMenu";

export default function Layout({ children }) {
  return (
    <>
      <LayoutMenu />
      {process.env.NODE_ENV === "development" && (
        <div className="fixed-top text-center pe-none">
          <h3>
            <span className="d-block d-sm-none">XS</span>
            <span className="d-none d-sm-block d-md-none">SM</span>
            <span className="d-none d-md-block d-lg-none">MD</span>
            <span className="d-none d-lg-block d-xl-none">LG</span>
            <span className="d-none d-xl-block d-xxl-none">XL</span>
            <span className="d-none d-xxl-block">XLL</span>
          </h3>
        </div>
      )}
      <div className="page-wrapper">
        <Outlet />
      </div>
    </>
  );
}
