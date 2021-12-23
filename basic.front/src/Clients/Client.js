import { useParams, Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { apiUrl, useApiFetch } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import MobilePageTitle from "../Generic/MobilePageTitle";
import clsx from "clsx";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

export default function Client({ backTo = null }) {
  const { clientId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(apiUrl("Clients", clientId), get, {});
  const [, links] = useApiFetch(apiUrl("Clients", clientId, "links"), get, {});

  return (
    <div className={clsx({ "container-xl": !backTo })}>
      <MobilePageTitle back={backTo}>
        <div className="navbar-brand flex-fill">{entity.displayName}</div>
        <Link to="edit" className="btn btn-primary">
          <IconEdit />
          Edit
        </Link>
      </MobilePageTitle>
      <div className="page-header d-none d-lg-block">
        <div className="row align-items-center">
          {backTo && (
            <div className="col-auto ms-auto d-print-none">
              <div className="d-flex">
                <Link
                  to={backTo}
                  className="btn btn-outline-primary btn-icon d-none d-lg-block"
                >
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
            <h2 className="page-title">{entity.displayName}</h2>
          </div>
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link to="edit" className="btn btn-primary d-none d-md-block">
                <IconEdit />
                Edit
              </Link>
              <Link
                to="edit"
                className="btn btn-primary btn-icon d-md-none"
                aria-label="Edit"
              >
                <IconEdit />
              </Link>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <Sections>
          <Section
            code="detail"
            element={
              <EntityDetail definitionName="ClientForView" entity={entity} />
            }
          >
            Detail
          </Section>
          <Section
            code="agreements"
            element={
              <div className="card">
                <div className="card-body"></div>
              </div>
            }
          >
            Agreements
            <span className="badge bg-green ms-2">{links.agreements}</span>
          </Section>
        </Sections>
      </div>
    </div>
  );
}
