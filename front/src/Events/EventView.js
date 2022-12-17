import { IconCheck, IconX } from "@tabler/icons";
import dayjs from "dayjs";
import { NavLink, useLoaderData, useParams, useRevalidator } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import EntityList from "../Generic/EntityList";
import AttachmentList from "../Attachments/AttachmentList";
import { useInRole } from "../Authentication";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

function EventAttachmentList() {
  const definition = useDefinition("AttachmentForList");
  const { eventId } = useParams();
  const [loading, elements] = useApiFetch(["Events", eventId, "Attachments"], { method: "GET" }, []);
  return (
    <div className="card">
      <AttachmentList
        loading={loading}
        definition={definition}
        entities={elements}
        baseTo="/attachment"
        typeOfParent="events"
        parentId={eventId}
      />
    </div>
  );
}

function EventViewDetail() {
  const definition = useDefinition("EventForView", transform);
  const entity = useLoaderData();

  return (
    <>
      <EntityDetail definition={definition} entity={entity} />
    </>
  );
}

function StatusList() {
  const definition = useDefinition("ModelStatusForList");
  const { eventId } = useParams();
  const [loading, elements] = useApiFetch(["Events", eventId, "statuses"], { method: "GET" }, []);

  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} elements={elements} />
    </div>
  );
}

export function EventView({ backTo = null, full = false }) {
  const revalidator = useRevalidator();
  const { eventId } = useParams();
  const get = { method: "GET" };
  const entity = useLoaderData();
  const [, next] = useApiFetch(["Events", eventId, "Statuses/Next"], get, []);
  const [, links] = useApiFetch(["Events", eventId, "links"], get, {});
  const isInRole = useInRole();

  function handleStatusChange(newStatus) {
    apiFetch(["events", eventId, "statuses"], {
      method: "POST",
      body: JSON.stringify({ from: entity.currentStatus.identifier, to: newStatus.identifier }),
    }).then(() => {
      entity.currentStatus = newStatus;
      revalidator.revalidate();
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
            <NavLink key={index} to={backTo}>
              <button key={index} className="btn btn-success mx-1" onClick={() => handleStatusChange(status)}>
                <IconCheck /> Approve
              </button>
            </NavLink>
          );
        case "Rejected":
          return (
            <NavLink key={index} to={backTo}>
              <button key={index} className="btn btn-danger mx-1" onClick={() => handleStatusChange(status)}>
                <IconX /> Reject
              </button>
            </NavLink>
          );
        case "Canceled":
          return (
            <NavLink key={index} to={backTo}>
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
        <Section code="detail" element={<EventViewDetail />}>
          Detail
        </Section>
        <Section code="statuses" element={<StatusList />}>
          Statuses
        </Section>
        {isInRole("beta") && (
          <Section code="attachments" element={<EventAttachmentList />}>
            Attachments
            <span className="badge ms-2 bg-green">{links.attachments || ""}</span>
          </Section>
        )}
      </Sections>
    </PageView>
  );
}
