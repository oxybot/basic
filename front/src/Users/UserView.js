import { Link, useLoaderData, useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import AttachmentList from "../Attachments/AttachmentList";
import { useInRole } from "../Authentication";

const get = { method: "GET" };

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "roles");
  return d;
};

function UserAttachmentList() {
  const definition = useDefinition("AttachmentForList");
  const { userId } = useParams();
  const [loading, elements] = useApiFetch(["Users", userId, "Attachments"], get, []);
  return (
    <div className="card">
      <AttachmentList
        loading={loading}
        definition={definition}
        entities={elements}
        baseTo="/attachment"
        typeOfParent="users"
        parentId={userId}
      />
    </div>
  );
}

function UserViewDetail() {
  const definition = useDefinition("UserForView", transform);
  const entity = useLoaderData();
  const isInRole = useInRole();
  const roles = entity.roles || [];

  return (
    <>
      <EntityDetail definition={definition} entity={entity} />
      <div className="card mb-3">
        <div className="card-header">
          <h3 className="card-title">Roles</h3>
          <span className="badge ms-2 bg-green">{roles.length || ""}</span>
          {isInRole("users") && (
            <Link to="roles" className="btn btn-outline-primary ms-auto">
              Set roles
            </Link>
          )}
        </div>
        <div className="card-body">
          {roles.length === 0 && (
            <p>
              <em>No specific role assigned</em>
            </p>
          )}
          {roles.map((role, index) => (
            <div key={index} className="badge me-2">
              {role.code}
            </div>
          ))}
        </div>
      </div>
    </>
  );
}

export function UserView({ backTo = null, full = false }) {
  const { userId } = useParams();
  const entity = useLoaderData();
  const [, links] = useApiFetch(["Users", userId, "links"], get, {});
  const isInRole = useInRole();

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="users">
      <Sections>
        <Section code="detail" element={<UserViewDetail />}>
          Detail
        </Section>
        {isInRole("beta") && (
          <Section code="attachments" element={<UserAttachmentList />}>
            Attachments
            <span className="badge ms-2 bg-green">{links.attachments || ""}</span>
          </Section>
        )}
      </Sections>
    </PageView>
  );
}
