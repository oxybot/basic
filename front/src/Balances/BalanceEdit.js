import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import DetailsForm from "./DetailsForm";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "details");
  return d;
};

function transform(e) {
  let updated = { ...e, userIdentifier: e.user.identifier, categoryIdentifier: e.category.identifier };
  delete updated.user;
  delete updated.category;
  return updated;
}

export function BalanceEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { balanceId } = useParams();
  const definition = useDefinition("BalanceForEdit", transformDef);
  const texts = {
    title: "Edit a Balance",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Balances"
      entityId={balanceId}
      extendedForm={(e, s, err) => <DetailsForm entity={e} setEntity={s} errors={err} />}
      onUpdate={handleUpdate}
      transform={transform}
    />
  );
}
