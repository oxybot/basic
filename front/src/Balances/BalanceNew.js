import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function BalanceNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("BalanceForEdit");
  const texts = {
    title: "Balances",
    subTitle: "Add a new Balance",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="Balances" texts={texts} onCreate={handleCreate} />;
}
