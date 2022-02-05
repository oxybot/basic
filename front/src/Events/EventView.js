import { IconCheck, IconX } from "@tabler/icons";
import { useDispatch } from "react-redux";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import { refresh } from "./slice";

function EventViewDetail({ entity }) {
  const definition = useDefinition("EventForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

function StatusList({ eventId }) {
  const definition = useDefinition("ModelStatusForList");
  const [loading, elements] = useApiFetch(["Events", eventId, "statuses"], { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} entities={elements} baseTo="/agreement" />
    </div>
  );
}

export function EventView({ backTo = null, full = false }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { eventId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["Events", eventId], get, {});

  function handleStatusChange(newStatus) {
    apiFetch(["events", eventId, "statuses"], {
      method: "POST",
      body: JSON.stringify({ from: entity.currentStatus.identifier, to: newStatus }),
    }).then(() => {
      navigate("../" + eventId);
      dispatch(refresh());
    });
  }
  function ExtraMenu() {
    if (!entity.currentStatus) {
      return null;
    }

    switch (entity.currentStatus.displayName) {
      case "Requested":
        return (
          <>
            <button
              className="btn btn-success mx-1"
              onClick={() => handleStatusChange("4151c014-ddde-43e4-aa7e-b98a339bbe74")}
            >
              <IconCheck /> Approve
            </button>
            <button
              className="btn btn-danger mx-1"
              onClick={() => handleStatusChange("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c")}
            >
              <IconX /> Reject
            </button>
          </>
        );

      default:
        return null;
    }
  }

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="noedit" extraMenu={<ExtraMenu />}>
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
