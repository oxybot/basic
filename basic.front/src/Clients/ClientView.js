import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";

function ClientViewDetail({ entity }) {
  return <EntityDetail definitionName="ClientForView" entity={entity} />;
}

function ClientViewAgreements({ clientId }) {
  const definition = useDefinition("AgreementForList");
  const url = apiUrl("Agreements");
  url.searchParams.set("clientId", clientId);
  const [loading, elements] = useApiFetch(url, { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList
        loading={loading}
        definition={definition}
        entities={elements}
        baseTo="/agreement"
      />
    </div>
  );
}

export default function ClientView({ backTo = null, full = false }) {
  const { clientId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(apiUrl("Clients", clientId), get, {});
  const [, links] = useApiFetch(apiUrl("Clients", clientId, "links"), get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity}>
      <Sections>
        <Section code="detail" element={<ClientViewDetail entity={entity} />}>
          Detail
        </Section>
        <Section
          code="agreements"
          element={<ClientViewAgreements clientId={clientId} />}
        >
          Agreements
          <span className="badge ms-2 bg-green">{links.agreements || ""}</span>
        </Section>
      </Sections>
    </PageView>
  );
}
