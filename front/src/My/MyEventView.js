import dayjs from "dayjs";
import { useLoaderData, useParams, useRevalidator } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import { useDispatch } from "react-redux";
import { addError } from "../Alerts/slice";
import { disconnect } from "../Authentication";
import { EventExtraMenu } from "../Events/components";
import Modal from "../Generic/Modal";

function EventViewDetail() {
  const definition = useDefinition("EventForView");
  const entity = useLoaderData();

  if (definition) {
    definition.fields = definition.fields.filter((f) => f.name !== "user");
  }

  return <EntityDetail definition={definition} entity={entity} />;
}

function StatusList({ eventId }) {
  const definition = useDefinition("ModelStatusForList");
  const [loading, elements] = useApiFetch(["My/Events", eventId, "Statuses"], { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} elements={elements.values} />
    </div>
  );
}

export function EventModals({ entity, next, onStatusChange }) {
  if (next.length === 0) {
    return null;
  }

  return next.map((status, index) => {
    switch (status.displayName) {
      case "Canceled":
        return (
          <Modal
            key={index}
            id="modal-cancel"
            title="Are you sure?"
            text={`Do you really want to cancel this ${entity.category.displayName} request starting ${dayjs(
              entity.startDate
            ).format("DD MMM YYYY")}?`}
            confirm="Yes"
            cancel="No"
            onConfirm={() => onStatusChange(status)}
          />
        );
      case "Approved":
      case "Rejected":
      default:
        return null;
    }
  });
}

export function MyEventView({ backTo = null, full = false }) {
  const revalidator = useRevalidator();
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const entity = useLoaderData();
  const [, next] = useApiFetch(["My", "Events", eventId, "Statuses/Next"], { method: "GET" }, []);

  function handleStatusChange(newStatus) {
    apiFetch(["my", "events", eventId, "statuses"], {
      method: "POST",
      body: JSON.stringify({ from: entity.currentStatus.identifier, to: newStatus.identifier }),
    })
      .then(() => {
        entity.currentStatus = newStatus;
        revalidator.revalidate();
      })
      .catch((err) => {
        if (err !== null && err.message === "401") {
          dispatch(disconnect());
        } else {
          console.error(err);
          dispatch(addError("Can't send this request", err.from));
        }
      });
  }

  return (
    <>
      <PageView
        backTo={backTo}
        full={full}
        entity={entity}
        title={dayjs(entity.startDate).format("DD MMM YYYY")}
        editRole="noedit"
        extraMenu={<EventExtraMenu next={next} onStatusChange={handleStatusChange} />}
      >
        <Sections>
          <Section code="detail" element={<EventViewDetail />}>
            Detail
          </Section>
          <Section code="statuses" element={<StatusList eventId={eventId} />}>
            Statuses
          </Section>
        </Sections>
      </PageView>
      <EventModals next={next} entity={entity} onStatusChange={handleStatusChange} />
    </>
  );
}
