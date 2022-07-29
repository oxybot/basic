import { IconCheck, IconX } from "@tabler/icons";
import dayjs from "dayjs";
import { useDispatch } from "react-redux";
import { NavLink, useParams } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import { refresh } from "./slice";
import EntityList from "../Generic/EntityList";

import EntityFieldView from "../Generic/EntityFieldView";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};
function EventViewDetail({ entity }) {
  const definition = useDefinition("EventForView", transform);

  const attachments = entity.attachments || []; 

  return (
    <>
    <EntityDetail definition={definition} entity={entity} />
    <div className="card mb-3">
        <div className="card-header">
          <h3 className="card-title">Attachments</h3>
          <span className="badge ms-2 bg-green">{attachments.length || ""}</span>
        </div>
        <div className="card-body">
          {attachments.length === 0 && (
            <p>
              <em>No attachment</em>
            </p>
          )}
          {attachments.map((attachment, index) => (
            <div key={index} className="row">
              <div className="col mb-3">
                <div className="lead">{attachment.displayName}</div>
                <div className="text-muted">
                  {attachment.displayName || <em title="No linked to an existing product">Specific product</em>}
                </div>
                <div className="text-muted">
                  <span>
                    <EntityFieldView type="string" value={attachment.displayName} />
                  </span>
                </div>
              </div>
              <div className="col-auto">
                <span className="h4 border-secondary">
                  <EntityFieldView type="image" value={attachment.attachment} />
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}

function StatusList({ eventId }) {
  const definition = useDefinition("ModelStatusForList");
  const [loading, elements] = useApiFetch(["Events", eventId, "statuses"], { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} entities={elements} />
    </div>
  );
}

export function EventView({ backTo = null, full = false }) {
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["Events", eventId], get, {});
  const [, next] = useApiFetch(["Events", eventId, "Statuses/Next"], get, []);

  function handleStatusChange(newStatus) {
    apiFetch(["events", eventId, "statuses"], {
      method: "POST",
      body: JSON.stringify({ from: entity.currentStatus.identifier, to: newStatus.identifier }),
    }).then(() => {
      entity.currentStatus = newStatus;
      dispatch(refresh());
    });
  }

  function ExtraMenu() {
    if (next.length === 0) {
      return null;
    }

    return next.map((status, index) => {
      switch (status.displayName) {
        case "Approved":
          return (
            <NavLink to={backTo}>
              <button key={index} className="btn btn-success mx-1" onClick={() => handleStatusChange(status)}>
                <IconCheck /> Approve
              </button>
            </NavLink>
          );
        case "Rejected":
          return (
            <NavLink to={backTo}>
              <button key={index} className="btn btn-danger mx-1" onClick={() => handleStatusChange(status)}>
                <IconX /> Reject
              </button>
            </NavLink>
          );
        case "Canceled":
          return (
            <NavLink to={backTo}>
              <button key={index} className="btn btn-outline-primary mx-1" onClick={() => handleStatusChange(status)}>
                <IconX /> Cancel
              </button>
            </NavLink>
          );
      default:
        return null;
      }
    });
  }

  return (
    <PageView
      backTo={backTo}
      full={full}
      title={entity.user ? entity.user.displayName + " - " + dayjs(entity.startDate).format("DD MMM YYYY") : null}
      entity={entity}
      editRole="noedit"
      extraMenu={<ExtraMenu />}
    >
      <Sections>
        <Section code="detail" element={<EventViewDetail entity={entity} />}>
          Detail
        </Section>
        <Section code="statuses" element={<StatusList eventId={eventId} />}>
          Statuses
        </Section>
      </Sections>
    </PageView>
  );
}
