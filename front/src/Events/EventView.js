import { IconCheck, IconX } from "@tabler/icons";
import dayjs from "dayjs";
import { useLoaderData, useParams, useRevalidator } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import EntityList from "../Generic/EntityList";
import AttachmentList from "../Attachments/AttachmentList";
import { useInRole } from "../Authentication";
import Modal from "../Generic/Modal";

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

  return <EntityDetail definition={definition} entity={entity} />;
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
            <button key={index} className="btn btn-success mx-1" onClick={() => handleStatusChange(status)}>
              <IconCheck /> Approve
            </button>
          );
        case "Rejected":
          return (
            <button key={index} className="btn btn-danger mx-1" data-bs-toggle="modal" data-bs-target="#modal-reject">
              <IconX /> Reject
            </button>
          );
        case "Canceled":
          return (
            <button key={index} className="btn btn-warning mx-1" data-bs-toggle="modal" data-bs-target="#modal-cancel">
              <IconX /> Cancel
            </button>
          );
        default:
          return null;
      }
    });
  }

  function Modals() {
    if (next.length === 0) {
      return null;
    }

    return next.map((status, index) => {
      switch (status.displayName) {
        case "Approved":
          return null;
        case "Rejected":
          return (
            <Modal
              id="modal-reject"
              title="Are you sure?"
              text="Do you really want to reject this request from XXX of XX days of XXX?"
              confirm="Reject"
              onConfirm={() => handleStatusChange(status)}
            />
          );
        case "Canceled":
          return (
            <Modal
              id="modal-cancel"
              title="Are you sure?"
              text="Do you really want to cancel this request from XXX of XX days of XXX?"
              confirm="Yes"
              cancel="No"
              onConfirm={() => handleStatusChange(status)}
            />
          );
        default:
          return null;
      }
    });
  }

  return (
    <>
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
      <Modals />
    </>
  );
}
