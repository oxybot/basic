import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";
import PageView from "../Generic/PageView";

function ProductViewDetail({ entity }) {
  return <EntityDetail definitionName="ProductForView" entity={entity} />;
}

export function ProductView({ backTo = null, full = false }) {
  const { productId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(apiUrl("Products", productId), get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity}>
      <Sections>
        <Section code="detail" element={<ProductViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
