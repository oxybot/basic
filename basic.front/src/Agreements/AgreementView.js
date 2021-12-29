import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Section from "../Generic/Section";
import Sections from "../Generic/Sections";

function AgreementViewDetail({ entity }) {
  const definition = useDefinition("AgreementForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

export function AgreementView({ backTo = null, full = false }) {
  const { agreementId } = useParams();
  const [, entity] = useApiFetch(apiUrl("Agreements", agreementId), { method: "GET" }, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} title={entity.title}>
      <Sections>
        <Section code="detail" element={<AgreementViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
