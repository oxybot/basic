import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export function useFiltering() {
  const [searchParams, setSearchParams] = useSearchParams();

  const filters = useMemo(() => {
    if (searchParams && searchParams.getAll("f")) {
      return Object.fromEntries(searchParams.getAll("f").map((e) => e.split("/")));
    } else {
      return null;
    }
  }, [searchParams]);

  const setFilters = function (updatedFilters) {
    console.log(updatedFilters);
    let updated = new URLSearchParams(searchParams);
    updated.delete("f");
    Object.entries(updatedFilters).forEach((p) => {
      if (p[1] !== null) {
        updated.set("f", p[0] + "/" + p[1]);
      }
    });

    setSearchParams(updated);
  };

  return [filters, setFilters];
}
