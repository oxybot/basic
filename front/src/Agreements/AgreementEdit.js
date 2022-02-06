import { useCallback, useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import ItemsForm from "./ItemsForm";
import { refresh } from "./slice";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "items");
  return d;
};

export function AgreementEdit({ full = false }) {
  const dispatch = useDispatch();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForEdit", transformDef);

  const transform = useCallback((e) => {
    const updated = { ...e, clientIdentifier: e.client.identifier };
    updated.items = e.items.map((i) => {
      const updatedItem = { ...i, productIdentifier: i.product?.identifier };
      delete updatedItem.product;
      return updatedItem;
    });

    delete updated.client;
    return updated;
  }, []);

  const [, entity] = useApiFetch(["Agreements", agreementId], { method: "GET" }, {}, transform);

  const texts = {
    title: (e) => e.title,
    subTitle: "Edit an Agreement",
    "form-action": "Update",
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
      baseApiUrl="Agreements"
      entityId={agreementId}
      extendedForm={(e, s, err) => <ItemsForm entity={e} setEntity={s} errors={err} />}
      onUpdate={handleUpdate}
    ></PageEdit>
  );
}
