import { useLoaderData } from "react-router-dom";
import { useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import PageView from "../Generic/PageView";

function ProductViewDetail() {
  const definition = useDefinition("ProductForView");
  const entity = useLoaderData();
  return <EntityDetail definition={definition} entity={entity} />;
}

export function ProductView({ backTo = null, full = false }) {
  const entity = useLoaderData();

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="clients">
      <Sections>
        <Section code="detail" element={<ProductViewDetail />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
