import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import AttachmentList from "../Attachments/AttachmentList";

const get = { method: "GET" };

function UserAttachmentList({ userId }) {
  const definition = useDefinition("AttachmentForList");
  const [loading, elements] = useApiFetch(["Users", userId, "Attachments"], get, []);
  return (
    <div className="card">
      <AttachmentList loading={loading} definition={definition} entities={elements} baseTo="/attachment" typeOfParent="users" parentId={userId} />
    </div>
  );
}

function UserViewDetail({ entity }) {
  const definition = useDefinition("UserForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

export function UserView({ backTo = null, full = false }) {
  const { userId } = useParams();
  const [, entity] = useApiFetch(["Users", userId], get, {});
  const [, links] = useApiFetch(["Users", userId, "links"], get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="user">
      <Sections>
        <Section code="detail" element={<UserViewDetail entity={entity} />}>
          Detail
        </Section>
        <Section code="attachments" element={<UserAttachmentList userId={userId} />}>
          Attachments
          <span className="badge ms-2 bg-green">{links.attachments || ""}</span>
        </Section>
      </Sections>
    </PageView>
  );
}
