import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import DetailsForm from "./DetailsForm";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "details");
  return d;
};

export function BalanceNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("BalanceForEdit", transformDef);
  const texts = {
    title: "Balances",
    subTitle: "Add a new Balance",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return (
    <PageNew
      definition={definition}
      baseApiUrl="Balances"
      texts={texts}
      extendedForm={(e, s, err) => <DetailsForm entity={e} setEntity={s} errors={err} />}
      onCreate={handleCreate}
    />
  );
}
