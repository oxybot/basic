import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import PageView from "../Generic/PageView";

function UserViewDetail({ entity }) {
  return <EntityDetail definitionName="UserForView" entity={entity} />;
}

export function UserView({ backTo = null, full = false }) {
  const { userId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(apiUrl("Users", userId), get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity}>
      <Sections>
        <Section code="detail" element={<UserViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
