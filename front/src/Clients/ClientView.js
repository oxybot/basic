import { useLoaderData, useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";

function ClientViewDetail() {
  const definition = useDefinition("ClientForView");
  const entity = useLoaderData();
  return <EntityDetail definition={definition} entity={entity} />;
}

function ClientViewAgreements() {
  const definition = useDefinition("AgreementForList");
  const { clientId } = useParams();
  const url = apiUrl("Agreements");
  url.searchParams.set("clientId", clientId);
  const [loading, elements] = useApiFetch(url, { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} elements={elements} baseTo="/agreement" />
    </div>
  );
}

export function ClientView({ backTo = null, full = false }) {
  const { clientId } = useParams();
  const get = { method: "GET" };
  const entity = useLoaderData();
  const [, links] = useApiFetch(["Clients", clientId, "links"], get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="client">
      <Sections>
        <Section code="detail" element={<ClientViewDetail />}>
          Detail
        </Section>
        <Section code="agreements" element={<ClientViewAgreements />}>
          Agreements
          <span className="badge ms-2 bg-green">{links.agreements || ""}</span>
        </Section>
      </Sections>
    </PageView>
  );
}
