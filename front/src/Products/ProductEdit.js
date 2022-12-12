import { useParams, useRevalidator } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function ProductEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { productId } = useParams();
  const definition = useDefinition("ProductForEdit");
  const [, entity] = useApiFetch(["Products", productId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Product",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Products"
      entityId={productId}
      onUpdate={handleUpdate}
    />
  );
}
