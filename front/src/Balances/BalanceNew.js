import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

export function BalanceNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("BalanceForEdit");
  const texts = {
    title: "Balances",
    subTitle: "Add a new Balance",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return <PageNew definition={definition} baseApiUrl="Balances" texts={texts} onCreate={handleCreate} />;
}
