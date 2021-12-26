import { apiUrl, useApiFetch, useDefinition } from "../api";
import { useParams } from "react-router-dom";
import PageEdit from "../Generic/PageEdit";

export function ProductEdit({full = false}) {
  const { productId } = useParams();
  const definition = useDefinition("ProductForEdit");
  const [, entity] = useApiFetch(
    apiUrl("Products", productId),
    { method: "GET" },
    {}
  );
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Product",
  };

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Products"
      entityId={productId}
    />
  );
}
