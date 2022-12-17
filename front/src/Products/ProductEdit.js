import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function ProductEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { productId } = useParams();
  const definition = useDefinition("ProductForEdit");
  const texts = {
    title: (e) => e.displayName,
    subTitle: "Edit a Product",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Products"
      entityId={productId}
      onUpdate={handleUpdate}
    />
  );
}
