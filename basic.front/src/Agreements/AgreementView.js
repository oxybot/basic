import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Section from "../Generic/Section";
import Sections from "../Generic/Sections";

function AgreementViewDetail({ entity }) {
  return <EntityDetail definitionName="AgreementForView" entity={entity} />;
}

export default function AgreementView({ backTo = null }) {
  const { agreementId } = useParams();
  const [, entity] = useApiFetch(
    apiUrl("Agreements", agreementId),
    { method: "GET" },
    {}
  );

  return (
    <PageView backTo={backTo} entity={entity} title={entity.title}>
      <Sections>
        <Section
          code="detail"
          element={<AgreementViewDetail entity={entity} />}
        >
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}