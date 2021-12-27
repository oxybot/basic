import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { refresh } from "./slice";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function ProductEdit({ full = false }) {
  const dispatch = useDispatch();
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

  function handleUpdate() {
    dispatch(refresh());
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
