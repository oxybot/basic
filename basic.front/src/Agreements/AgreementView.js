import pluralize from "pluralize";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import EntityField from "../Generic/EntityField";
import PageView from "../Generic/PageView";
import Section from "../Generic/Section";
import Sections from "../Generic/Sections";
import { toCurrency } from "../helpers";

function AgreementViewDetail({ entity }) {
  const definition = useDefinition("AgreementForView", (d) => {
    d.fields = d.fields.filter((i) => i.name !== "items");
    return d;
  });

  const items = entity.items || [];

  return (
    <>
      <EntityDetail definition={definition} entity={entity} />
      <div className="card mb-3">
        <div className="card-header">
          <h3 className="card-title">Items</h3>
          <span className="badge ms-2 bg-green">{items.length || ""}</span>
          <span className="h2 mb-0 ms-auto">{toCurrency(items.reduce((a, c) => a + c.totalPrice, 0))}</span>
        </div>
        <div className="card-body">
          {items.length === 0 && (
            <p>
              <em>No item defined</em>
            </p>
          )}
          {items.map((item, index) => (
            <div key={index} className="row">
              <div className="col mb-3">
                <div className="lead">{item.description}</div>
                <div className="text-muted">
                  {item.product?.displayName || <em title="No linked to an existing product">Specific product</em>}
                </div>
                <div className="text-muted">
                  <span>{pluralize("unit", item.quantity, true)}</span>
                  <span className="mx-1">x</span>
                  <span>
                    <EntityField type="currency" value={item.unitPrice} />
                  </span>
                </div>
              </div>
              <div className="col-auto">
                <span className="h4 border-secondary">
                  <EntityField type="currency" value={item.totalPrice} />
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}

export function AgreementView({ backTo = null, full = false }) {
  const { agreementId } = useParams();
  const [, entity] = useApiFetch(["Agreements", agreementId], { method: "GET" }, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} title={entity.title} editRole="client">
      <Sections>
        <Section code="detail" element={<AgreementViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
