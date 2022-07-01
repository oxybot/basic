import { Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { useInRole } from "../Authentication";
import MobilePageTitle from "../Generic/MobilePageTitle";
import clsx from "clsx";

export default function PageView({
  backTo = null,
  entity,
  full = false,
  title = null,
  editRole = null,
  children,
  extraMenu = null,
}) {
  const isInRole = useInRole();

  if (title === null) {
    title = entity.displayName;
  }

  return (
    <div className={clsx({ "container-xl": full })}>
      <MobilePageTitle back={backTo}>
        <div className="navbar-brand flex-fill">{title}</div>
        {isInRole(editRole) && (
          <Link to="edit" className="btn btn-primary btn-icon" arial-label="Edit">
            <IconEdit />
          </Link>
        )}
        {extraMenu}
      </MobilePageTitle>
      <div className="page-header d-none d-lg-block">
        <div className="row align-items-center">
          {backTo && (
            <div className="col-auto ms-auto d-print-none">
              <div className="d-flex">
                <Link to={backTo} className="btn btn-outline-primary btn-icon d-none d-lg-block">
                  <IconChevronRight />
                </Link>
                <Link to={backTo} className="btn btn-outline-primary d-lg-none">
                  <IconChevronLeft />
                  Back
                </Link>
              </div>
            </div>
          )}
          <div className="col">
            <h2 className="page-title">{title}</h2>
          </div>
          <div className="col-auto ms-auto d-print-none">
            {isInRole(editRole) && (
              <>
                <Link to="edit" className="btn btn-primary mx-1 d-none d-md-inline-flex">
                  <IconEdit />
                  Edit
                </Link>
                <Link to="edit" className="btn btn-primary mx-1 btn-icon d-md-none" aria-label="Edit">
                  <IconEdit />
                </Link>
              </>
            )}
            {extraMenu}
          </div>
        </div>
      </div>
      <div className="page-body">{children}</div>
    </div>
  );
}