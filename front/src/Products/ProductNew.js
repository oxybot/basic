import { useDispatch } from "react-redux";
import { retrieveAll } from "./slice";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function ProductNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("ProductForEdit");
  const texts = {
    title: "Products",
    subTitle: "Add a new Product",
  };

  function handleCreate() {
    dispatch(retrieveAll());
  }

  return <PageNew definition={definition} baseApiUrl="Products" texts={texts} onCreate={handleCreate} />;
}
