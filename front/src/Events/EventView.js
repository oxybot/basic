import dayjs from "dayjs";
import { useLoaderData, useParams, useRevalidator } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import EntityList from "../Generic/EntityList";
import AttachmentList from "../Attachments/AttachmentList";
import { disconnect, useInRole } from "../Authentication";
import { EventExtraMenu, EventModals } from "./components";
import { useDispatch } from "react-redux";
import { addError } from "../Alerts/slice";

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

function EventViewDetail({ entity }) {
  const definition = useDefinition("EventForView", transform);
  return <EntityDetail definition={definition} entity={entity} />;
}

function StatusList() {
  const definition = useDefinition("ModelStatusForList");
  const { eventId } = useParams();
  const [loading, elements] = useApiFetch(["Events", eventId, "statuses"], { method: "GET" }, []);

  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} elements={elements.values} />
    </div>
  );
}

export function EventView({ backTo = null, full = false }) {
  const revalidator = useRevalidator();
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const get = { method: "GET" };
  const [entity, next] = useLoaderData();
  const [, links] = useApiFetch(["Events", eventId, "links"], get, {});
  const isInRole = useInRole();

  function handleStatusChange(newStatus) {
    apiFetch(["events", eventId, "statuses"], {
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
        title={entity.user ? entity.user.displayName + " - " + dayjs(entity.startDate).format("DD MMM YYYY") : null}
        entity={entity}
        editRole="noedit"
        extraMenu={<EventExtraMenu next={next} onStatusChange={handleStatusChange} />}
      >
        <Sections>
          <Section code="detail" element={<EventViewDetail entity={entity} />}>
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
      <EventModals next={next} entity={entity} onStatusChange={handleStatusChange} />
    </>
  );
}
