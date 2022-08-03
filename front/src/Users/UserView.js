import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import AttachmentList from "../Attachments/AttachmentList";

function EventViewAttachments({ userId }) {
  const host = "user";
  const definition = useDefinition("AttachmentForList");
  const url = apiUrl("Attachment/");
  url.searchParams.set('entityId', userId);
  url.searchParams.set('hostAttachment', host);
  const [loading, elements] = useApiFetch(url, { method: "GET" }, []);
  return (
    <div className="card">
      <AttachmentList loading={loading} definition={definition} entities={elements} baseTo="/attachment" />
    </div>
  );
}

function UserViewDetail({ entity }) {
  const definition = useDefinition("UserForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

export function UserView({ backTo = null, full = false }) {
  const { userId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["Users", userId], get, {});
  const [, links] = useApiFetch(["Events", userId, "links"], get, {});


  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="user">
      <Sections>
        <Section code="detail" element={<UserViewDetail entity={entity} />}>
          Detail
        </Section>
        <Section code="attachments" element={<EventViewAttachments userId={userId} />}>
          Attachments
          <span className="badge ms-2 bg-green">{links.attachments || ""}</span>
        </Section>
      </Sections>
    </PageView>
  );
}
